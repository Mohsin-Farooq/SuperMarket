using System;

namespace SuperMarketGame
{
    public static class CashCounterEvent
    {
        public static Action<float> OnAmountUpdate;
    }
}