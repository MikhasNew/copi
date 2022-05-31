using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.AppCompat.App;
using AndroidX.ViewPager2.Widget;
using System.Collections.Generic;
using AndroidX.ViewPager2.Adapter;
using AndroidX.Lifecycle;
using Google.Android.Material.Tabs;
using Google.Android.Material.Snackbar;
using Google.Android.Material.FloatingActionButton;
using static Google.Android.Material.Tabs.TabLayoutMediator;
using Android.Widget;
using Android.Provider;
using Android.Content;
using MyAndroidBankController.Parsers;

namespace MyAndroidBankController
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private static List<string> fragmentTitles;
        private static int[] tabIcons;
        private string tabText;
        TabLayout tabLayout;
        ViewPager2 pager;
        private static List<string> smsAdressFilters = new List<string>();

        #region Stock
        protected override void OnCreate(Bundle savedInstanceState)
        {
            #region Stock
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            AndroidX.AppCompat.Widget.Toolbar toolbar = FindViewById<AndroidX.AppCompat.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;
            #endregion

            #region ReadSmS
            smsAdressFilters.Add("ASB.BY");
            List<Sms> lst = GetAllSms(smsAdressFilters);
            ParseSmsToDb(lst);
            #endregion

            #region Castom Tab
            tabLayout = FindViewById<TabLayout>(Resource.Id.tabLayout);
            tabLayout.InlineLabel = true;
            var layoutParams = tabLayout.LayoutParameters as LinearLayout.LayoutParams;
            layoutParams.Weight = 0f;
            layoutParams.Width = LinearLayout.LayoutParams.WrapContent;
            tabLayout.LayoutParameters = layoutParams;

            pager = FindViewById<ViewPager2>(Resource.Id.pager);
            CustomViewPager2Adapter adapter = new CustomViewPager2Adapter(this.SupportFragmentManager, this.Lifecycle);
           
            fragmentTitles = new List<string>() {"A", "B", "C" };
            tabIcons = new int[]{
            Resource.Drawable.abc_ic_clear_material,
            Resource.Drawable.abc_ic_clear_material,
            Resource.Drawable.abc_ic_clear_material 
            };

            tabLayout.TabSelected += (object sender, TabLayout.TabSelectedEventArgs e) =>
            {
                var tab = e.Tab;
                var layout = tab.View;
                

                //var layoutParams = layout.LayoutParameters as LinearLayout.LayoutParams;
                //layoutParams.Weight = 0f;
                //layoutParams.Width = LinearLayout.LayoutParams.WrapContent;
                //layout.LayoutParameters = layoutParams;
                switch (tab.Position)
                {
                    case 0:
                        tab.SetText(DatesRepositorio.GetSum(DatesRepositorio.GetPayments()).ToString()); 
                        break;
                    case 1:
                        tab.SetText(DatesRepositorio.GetSum(DatesRepositorio.GetDeposits()).ToString());
                        break;
                    case 2:
                        tab.SetText(DatesRepositorio.GetSum(DatesRepositorio.GetCashs()).ToString());
                        break;
                }
              

            };
            tabLayout.TabUnselected += (object sender, TabLayout.TabUnselectedEventArgs e) =>
            {
                var tab = e.Tab;
                var layout = tab.View;
                tab.SetText("");
                var layoutParams = layout.LayoutParameters as LinearLayout.LayoutParams;
               // layoutParams.Weight = 0f;
                layoutParams.Width = LinearLayout.LayoutParams.WrapContent;
                layout.LayoutParameters = layoutParams;
            };
            pager.Adapter = adapter;
            adapter.NotifyDataSetChanged();

            new TabLayoutMediator(tabLayout, pager, new CustomStrategy()).Attach();
            #endregion
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View)sender;
            Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                .SetAction("Action", (View.IOnClickListener)null).Show();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        #endregion


        #region Castom Tab
        public class CustomViewPager2Adapter : FragmentStateAdapter
        {
            public CustomViewPager2Adapter(AndroidX.Fragment.App.FragmentManager fragmentManager, Lifecycle lifecycle) : base(fragmentManager, lifecycle)
            {

            }
            public override int ItemCount => 3;
            private AndroidX.Fragment.App.Fragment fragment = new AndroidX.Fragment.App.Fragment();
            public override AndroidX.Fragment.App.Fragment CreateFragment(int position)
            {
                switch (position)
                {
                    case 0:
                        fragment = new ViewPage2FragmentA();
                        break;
                    case 1:
                        fragment = new ViewPage2FragmentB();
                        break;
                    case 2:
                        fragment = new ViewPage2FragmentC();
                        break;
                }
                return fragment;
            }
        }
        public class ViewPage2FragmentA : AndroidX.Fragment.App.Fragment
        {
            public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
            {
                #region ListItem
                var ViewAD = LayoutInflater.Inflate(Resource.Layout.tab_layout, container, false);
                var listAdapter = new DataAdapter(this, DatesRepositorio.GetPayments());
                ViewAD.FindViewById<ListView>(Resource.Id.dateslistView).Adapter = listAdapter;
                return ViewAD;
                #endregion
            }
        }
        public class ViewPage2FragmentB : AndroidX.Fragment.App.Fragment
        {
            public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
            {
                #region ListItem
                var ViewAD = LayoutInflater.Inflate(Resource.Layout.tab_layout, container, false);
                var listAdapter = new DataAdapter(this, DatesRepositorio.GetDeposits());
                ViewAD.FindViewById<ListView>(Resource.Id.dateslistView).Adapter = listAdapter;
                return ViewAD;
                #endregion
            }
        }
        public class ViewPage2FragmentC : AndroidX.Fragment.App.Fragment
        {
            public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
            {
                #region ListItem
                var ViewAD = LayoutInflater.Inflate(Resource.Layout.tab_layout, container, false);
                var listAdapter = new DataAdapter(this, DatesRepositorio.GetCashs());
                ViewAD.FindViewById<ListView>(Resource.Id.dateslistView).Adapter = listAdapter;
                return ViewAD;
                #endregion
            }
        }
        public class CustomStrategy : Java.Lang.Object, ITabConfigurationStrategy
        {
            public void OnConfigureTab(TabLayout.Tab p0, int p1)
            {
               // p0.SetText(MainActivity.fragmentTitles[p1]);
                p0.SetIcon(MainActivity.tabIcons[p1]);
            }
        }
        #endregion

        #region SmS
        public List<Sms> GetAllSms(List<string> adressFilter)
        {
            List<Sms> lstSms = new List<Sms>();
            Sms objSms = new Sms();
            Android.Net.Uri message = Android.Net.Uri.Parse("content://sms/");

            using (var c = ContentResolver.Query(message, null, null, null, null))
            {

                //StartManagingCursor(c);
                int totalSMS = c.Count;
               
                if (c.MoveToFirst())
                {
                   
                    for (int i = 0; i < totalSMS; i++)
                    {
                        if (adressFilter.Contains(c.GetString(c.GetColumnIndexOrThrow("address"))))
                        {
                            objSms = new Sms();
                            objSms.setId(c.GetString(c.GetColumnIndexOrThrow("_id")));
                            objSms.setAddress(c.GetString(c.GetColumnIndexOrThrow("address")));
                            objSms.setMsg(c.GetString(c.GetColumnIndexOrThrow("body")));
                            objSms.setReadState(c.GetString(c.GetColumnIndex("read")));
                            objSms.setTime(c.GetString(c.GetColumnIndexOrThrow("date")));
                            if (c.GetString(c.GetColumnIndexOrThrow("type")).Contains("1"))
                            {
                                objSms.setFolderName("inbox");
                            }
                            else
                            {
                                objSms.setFolderName("sent");
                            }
                            lstSms.Add(objSms);
                        }
                        c.MoveToNext();
                    }
                }
                else
                {
                    lstSms.Add(new Sms());
                    return lstSms;
                }
                c.Close();
                return lstSms;
            }
        }
        public static void ParseSmsToDb(List<Sms> smsList)
        {
            ParserBelarusbank parserBelarusbank = new ParserBelarusbank(smsList);
            DatesRepositorio.AddDatas(parserBelarusbank.Data);
           
        }

    }
    #endregion
}

