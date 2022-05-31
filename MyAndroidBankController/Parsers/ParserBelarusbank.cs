using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.Globalization;

namespace MyAndroidBankController.Parsers
{
    internal class ParserBelarusbank
    {
        public List<DataItem> Data { get; }
        List<Sms> smslist = new List<Sms>();
        public ParserBelarusbank(List<Sms> data)
        {
            Data = new List<DataItem>();
            smslist = data;
            ToDataItems();
        }
        public void ToDataItems()
        {
            List<string> words = new List<string>();
            foreach (Sms sms in smslist)
            {
                var ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                ci.NumberFormat.NumberDecimalSeparator = ".";

                var msg = sms.getMsg();
                string wordsPattern = @"(?:[\w\.#:]{1,}|>)";
                Regex wordsRegex = new Regex(wordsPattern);
                var smsWords = wordsRegex.Matches(msg);

                var firstVal = smsWords[0].Value;
                if (Enum.IsDefined(typeof(OperacionTyps), smsWords[0].Value))
                {
                    OperacionTyps operType = (OperacionTyps)Enum.Parse(typeof(OperacionTyps), smsWords[0].Value.ToString());
                    var dateValue = smsWords.Where(x => x.Value.Equals("DATA")).First().NextMatch();
                    var timeValue = dateValue.NextMatch();
                    DataItem dataItem = new DataItem(operType, DateTime.Parse($"{dateValue} {timeValue}"));

                    for (int i = 0; i < smsWords.Count; i++)
                    {
                        if (smsWords[i].Value == "BYN")
                        {
                            dataItem.Sum = float.Parse(smsWords[i - 1].Value, ci);
                            break;
                        }
                    }

                    // var sumIndex = smsWords.Where(x => x.Value == "BYN").Select(x=>x.Index);
                    // dataItem.Sum = float.Parse(smsWords[sumIndex-1].Value, ci);

                    dataItem.Balance = float.Parse(smsWords[smsWords.Count - 2].Value, ci);
                    dataItem.Karta = int.Parse(smsWords.Where(x => x.Value.Equals("KARTA")).First().NextMatch().Value.Trim('#'), ci);
                    var dm = timeValue.NextMatch();
                    string discr = timeValue.NextMatch().Value;

                    if (operType != OperacionTyps.ZACHISLENIE)
                    {
                        while (dm.Value != ">")
                        {
                            dm = dm.NextMatch();
                            discr += $" {dm.Value}";
                        }

                        dataItem.Descripton = discr.Trim('>');
                    }

                    Data.Add(dataItem);
                }

            }


        }
    }
}