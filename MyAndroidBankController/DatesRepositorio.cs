using System;
using System.Linq;
using System.Collections.Generic;

namespace MyAndroidBankController
{
    public static class DatesRepositorio
    {
        public static List<DataItem> DataItems { get; private set; }
        public static void AddDatas(List<DataItem> dataItems)
        {
            DataItems = dataItems;
        }
        public static List<DataItem> GetPayments()
        {
            var payments = DataItems.Where(x => x.OperacionTyp == OperacionTyps.OPLATA).ToList();
           return DataItems.Where(x => x.OperacionTyp == OperacionTyps.OPLATA).ToList();
        }

        public static List<DataItem> GetDeposits()
        {
            return DataItems.Where(x => x.OperacionTyp == OperacionTyps.ZACHISLENIE).ToList();
        }
        public static List<DataItem> GetCashs()
        {
            return DataItems.Where(x => x.OperacionTyp == OperacionTyps.NALICHNYE).ToList();
        }

        public static float GetSum(List<DataItem> dataItems)
        {
            return dataItems.Select(x => x.Sum).Sum();
        }


        //static DatesRepositorio()
        //{
        //    DataItems = new List<DataItem>();
        //    for (int i = 0; i < 10; i++)
        //    {
        //        AddFilmes();
        //    }

        //}

        //private static void AddFilmes()
        //{

        //    DataItems.Add(new DataItem(OperacionTyps.OPLTA, new DateTime(1977, 05, 25))
        //    {
        //        Title = "A New Hope",
        //        Descripton = "George Lucas"
        //    });


        //}
    }
}