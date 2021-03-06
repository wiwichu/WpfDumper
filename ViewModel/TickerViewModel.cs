﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WpfDumper.Helpers;
using WpfDumper.Model;
using WpfDumper.Services;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.PlatformServices;
using System.Windows.Input;

namespace WpfDumper.ViewModel
{
    class TickerViewModel : ValidatorBase
    {
        private static ConcurrentDictionary<string, Tick> ticks = new ConcurrentDictionary<string, Tick>();
        DateTime lastTickUpdate = DateTime.UtcNow;
        private Timer tickTimer = null;
        private void tickTimerCallback(object state)
        {
            if ((DateTime.UtcNow.Subtract(lastTickUpdate)).TotalMilliseconds > refreshRate*1000)
            {
                lastTickUpdate = DateTime.UtcNow;
                OnPropertyChanged("Ticks");
            }
        }

        public TickerViewModel()
        {
            Initialize();
            LoadCommands();
        }
        private void LoadCommands()
        {
            SelectItemCommand = new RelayCommand(OnSelectItem, CanSelectItem);
        }

        private bool CanSelectItem(object obj)
        {
            return true;
        }

        private void OnSelectItem(object obj)
        {
            
        }

        private void Initialize()
        {
            //ticks.Clear();
            //for (int i = 0; i < 100; i++)
            //{
            //    Tick t = new Tick
            //    {
            //        Symbol = $"Sym{i}",
            //        TickValue = i,
            //        TimeStamp = DateTime.UtcNow
            //    };
            //    ticks.AddOrUpdate(t.Symbol, t, (key, oldValue) => t);
            //    OnPropertyChanged("Ticks");
            //}
            //var iTicks = ticks.Values.ToObservable();
            //iTicks
            //        //.SubscribeOn(NewThreadScheduler.Default)
            //        .ObserveOn(ThreadPoolScheduler.Instance)
            //        .Subscribe(ProcessTick);

            tickTimer = new Timer(tickTimerCallback, null, 50, 50);
            TickerService.Ticks
                    .SubscribeOn(NewThreadScheduler.Default)
                    .ObserveOn(ThreadPoolScheduler.Instance)
                    .Subscribe(ProcessTick);
        }
        private void ProcessTick(Tick tick)
        {
            Tick oldTick;
            if (!ticks.TryGetValue(tick.Symbol, out oldTick))
            {
                ticks.TryAdd(tick.Symbol, tick);
            }
            else
            {
                oldTick.TickValue = tick.TickValue;
                oldTick.TimeStamp = tick.TimeStamp;
                //ticks.TryUpdate(tick.Symbol, tick, tick);
            }
        }

        public ObservableCollection<Tick> Ticks
        {
            get
            {
                return new ObservableCollection<Tick>(ticks.Values.OrderBy((t)=>t.Symbol));
            }
        }
        private int refreshRate=1;
        public int RefreshRate
        {
            get
            {
                return refreshRate;
            }
            set
            {
                SetProperty(ref refreshRate, value);
            }
        }
        public ICommand SelectItemCommand
        {
            get;
            set;
        }

    }
}
