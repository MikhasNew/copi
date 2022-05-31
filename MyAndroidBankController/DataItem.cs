﻿using System;

namespace MyAndroidBankController
{

    public class DataItem
    {
        public int Id { get; set; }
        public long HashId { get; private set; }
        public OperacionTyps OperacionTyp { get; private set; }
        public float Balance { get; set; }
        public float Sum { get; set; }
        public int Karta { get; set; }
        public string Title { get; set; }
        public string Descripton { get; set; }
        public DateTime Date { get; private set; }
        public DataItem(OperacionTyps operacionTyp, DateTime dateTime )
        {
            Date = dateTime;
            OperacionTyp = operacionTyp;
            //not overflov to 16/11/3169 09:46:40
            HashId =(long)((int)operacionTyp * 1000000000000000000)+ dateTime.Ticks;
        }
        public override string ToString()
        {
            return $"{Sum} {Descripton} {Date} ";
        }


    }
}
