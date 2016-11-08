using UnityEngine;
using System.Collections;
using Assets.Backtory.core;
using System;
using System.Collections.Generic;

namespace Assets.BacktorySDK.core
{
    interface IDispatcher
    {
        void Invoke(Action fn);
    }

    public class BacktoryManager : UnitySingleton<BacktoryManager>, IDispatcher
    {
        public IGlobalEventListener GlobalEventListener { set; get; }

        #region dispatch
        internal IList<Action> pending = new List<Action>();

        //
        // Schedule code for execution in the main-thread.
        //
        public void Invoke(Action fn)
        {
            lock (pending)
            {
                pending.Add(fn);
            }
        }

        //
        // Execute pending actions.
        //
        internal void InvokePending()
        {
            lock (pending)
            {
                foreach (var action in pending)
                {
                    action(); // Invoke the action.
                }
                pending.Clear(); // Clear the pending list.
            }
        }

        // Update is called once per frame
        void Update()
        {
            Instance.InvokePending();
        }
        #endregion

        
    }


}