using System;
using System.Collections.Generic;

namespace test.Function
{
    public class ParseObject
    {
        public String[] array;
        public decimal testValue1;
        public decimal testValue2;

    }

    public class TermCashflowYieldSet
    {
        public decimal cashflow;
        public decimal term;
        public DateTime date;
        public SpotYield spotYield;
    }
    public class SpotYield
    {
        public Term Term;
        public decimal Yield;
    }

    public class PlainListTermCashFlowSet
    {
        public IList<TermCashflowYieldSet> cashflowSet;
        public DateTime anchorDate;
        public Term termType;
        public decimal nominal;
        public RESTMethodType restMethodType;
    }
    public enum Term
    {
        MonthlyEffective,
        YearlyEffective
    }
    public enum RESTMethodType
    {
        GET,
        POST
    }
}
