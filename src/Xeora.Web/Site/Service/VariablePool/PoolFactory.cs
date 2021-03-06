﻿using System.Collections.Concurrent;
using Xeora.Web.Basics.Service;
using Xeora.Web.Service.DSS;

namespace Xeora.Web.Site.Service
{
    public class PoolFactory : ConcurrentDictionary<string, IVariablePool>
    {
        private static PoolFactory _Current = null;
        private short _ExpiresInMinutes ;

        private PoolFactory(short expiresInMinutes) =>
            this._ExpiresInMinutes = expiresInMinutes;

        public static void Initialize(short expiresInMinutes)
        {
            if (PoolFactory._Current != null)
                return;

            PoolFactory._Current = new PoolFactory(expiresInMinutes);
        }

        public static void Get(string sessionID, string keyID, out IVariablePool variablePool)
        {
            if (PoolFactory._Current == null)
                throw new Exception.VariablePoolNotReadyException();

            PoolFactory._Current.ProvideVariablePool(sessionID, keyID, out variablePool);
        }

        public static void Copy(string keyID, string fromSessionID, string toSessionID)
        {
            if (PoolFactory._Current == null)
                throw new Exception.VariablePoolNotReadyException();

            PoolFactory._Current.CopyVariablePool(keyID, fromSessionID, toSessionID);
        }

        private string CreatePoolKey(string sessionID, string keyID) =>
            string.Format("{0}_{1}", sessionID, keyID);

        private void ProvideVariablePool(string sessionID, string keyID, out IVariablePool variablePool)
        {
            string poolKey = this.CreatePoolKey(sessionID, keyID);

            Basics.DSS.IDSS reservation;
            DSSManager.Current.Reserve(poolKey, this._ExpiresInMinutes, out reservation);

            variablePool = new VariablePool(sessionID, keyID, ref reservation);
        }

        private void CopyVariablePool(string keyID, string fromSessionID, string toSessionID)
        {
            IVariablePool oldVariablePool = null;
            this.ProvideVariablePool(fromSessionID, keyID, out oldVariablePool);

            IVariablePool newVariablePool = null;
            this.ProvideVariablePool(toSessionID, keyID, out newVariablePool);

            oldVariablePool.CopyInto(ref newVariablePool);
        }
    }
}
