using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Maps;
using System.Device.Location;
using Microsoft.Phone.Shell;
using System.Windows.Media.Imaging;

namespace blood_house
{
    public partial class MainPage : PhoneApplicationPage
    { 
        GeoCoordinateWatcher watcher;

        //헌혈의 집 운영정보 dictionary
        public static Dictionary<string, string> bh = new Dictionary<string, string>
            {
	        {"서울서부", "평일 : 09:00~18:00 \n전화번호 : 02-6711-0184"},
	        {"우장산역", "평일 : 09:00-18:00 \n전화번호 : 02-2603-5817"},
	        {"홍대", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 02-323-5420"},
	        {"이대", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 02-715-3105"},
	        {"신촌", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 02-392-6460"},
	        {"신촌연대", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 02-312-1247"},
	        {"서울역", "평일 : 09:00-18:00 \n전화번호 : 02-752-9020"},
	        {"대방역", "평일 : 09:00-18:00 \n전화번호 : 02-825-6560"},
	        {"구로디지털단지역", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 02-869-9415"},
	        {"신도림테크노마트", "평일 : 09:00-18:00 \n전화번호 : 02-861-0801"},
	        {"서울대역", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 02-873-4364"},
	        {"이수", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 02-578-9811"},
	        {"강남", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 02-533-0770"},
	        {"강남2", "평일 : 09:00-18:00 \n전화번호 : 02-564-1525"},
	        {"서울남부", "평일 : 09:00-18:00 \n전화번호 : 02-570-0662"},
	        {"잠실역", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 02-2202-7479"},
	        {"강남면허", "평일 : 09:00-18:00 \n전화번호 : 02-565-8332"},
	        {"코엑스", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 02-551-0600"},
	        {"천호", "평,토(일) : 09:00(10:00)~18:00 \n전화번호 : 02-485-3515"},
	        {"동서울", "평일 : 09:00-18:00 \n전화번호 : 02-2201-8481"},
	        {"동서울2", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 02-446-3526"},
	        {"건대역", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 02-498-4185\n서울 광진구 화양동 5-91 하마빌딩 4층\n(건대역 2번출구 건대맛거리입구 50m이내)"}
            };
        // Constructor       
        public MainPage()
        {
            InitializeComponent();
            // 메뉴바

                 ApplicationBar = new ApplicationBar();
                 ApplicationBarIconButton hereBt = new ApplicationBarIconButton();
                 hereBt.IconUri = new Uri("/icons/appbar.check.rest.png", UriKind.Relative);
                 hereBt.Text = "Here";
                 ApplicationBar.Buttons.Add(hereBt);

                 hereBt.Click += new EventHandler(hereBt_Click);

                 ApplicationBarMenuItem westBt = new ApplicationBarMenuItem("서울서부");
                 ApplicationBar.MenuItems.Add(westBt);

                 westBt.Click += new EventHandler(westBt_Click);

                 ApplicationBarMenuItem southBt = new ApplicationBarMenuItem("서울남부");
                 ApplicationBar.MenuItems.Add(southBt);

                 southBt.Click += new EventHandler(southBt_Click);


                 ApplicationBarMenuItem infoBt = new ApplicationBarMenuItem("앱 정보");
                 ApplicationBar.MenuItems.Add(infoBt);

                 infoBt.Click += new EventHandler(infoBt_Click);

             // GPS init
                watcher = new GeoCoordinateWatcher();
                watcher.MovementThreshold = 20;
                watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(watcher_PositionChanged);
            // Set the center coordinate and zoom level

                //서울남부
                GeoCoordinate nbh = new GeoCoordinate(37.4821632745255, 127.048666477203);//서울남부
                GeoCoordinate chs = new GeoCoordinate(37.5377759248814, 127.12706208229);//천호역
                GeoCoordinate dsu = new GeoCoordinate(37.534715, 127.094433);//동서울
                GeoCoordinate ds2 = new GeoCoordinate(37.5339662, 127.0936207);//동서울2
                GeoCoordinate jsy = new GeoCoordinate(37.514066147794594, 127.10201025009155);//잠실역
                GeoCoordinate isy = new GeoCoordinate(37.48645835851004, 126.98161125183105);//이수
                GeoCoordinate gns = new GeoCoordinate(37.50122322, 127.025599479675);//강남
                GeoCoordinate gn2 = new GeoCoordinate(37.4971545557763, 127.0283567905426);//강남2
                GeoCoordinate gnd = new GeoCoordinate(37.5080364599685, 127.066766023635);//강남면허
                GeoCoordinate cex = new GeoCoordinate(37.5117792, 127.0580198);//코엑스
                GeoCoordinate kds = new GeoCoordinate(37.5407449657831, 127.070789337158);//건대역

                //서울서부
                GeoCoordinate ssh = new GeoCoordinate(37.5481129, 126.8708138);//서울서부혈액원
                GeoCoordinate ujs = new GeoCoordinate(37.5476609263525, 126.836203336715);//우장산
                GeoCoordinate hd = new GeoCoordinate(37.5558491, 126.9228391);//홍대
                GeoCoordinate ld = new GeoCoordinate(37.5572165, 126.9456714);//이대
                GeoCoordinate sus = new GeoCoordinate(37.5560094138035, 126.971890926361);//서울역
                GeoCoordinate scs = new GeoCoordinate(37.55647327538, 126.93705525654);//신촌
                GeoCoordinate syf = new GeoCoordinate(37.5574595651406, 126.937386989593);//신촌연대
                GeoCoordinate dbs = new GeoCoordinate(37.5129896055201, 126.926196813583);//대방역
                GeoCoordinate stm = new GeoCoordinate(37.5079949, 126.8909477);//신림동테크노마트
                GeoCoordinate gds = new GeoCoordinate(37.4848152732836, 126.901220083236);//구로디지털단지
                GeoCoordinate sud = new GeoCoordinate(37.4785192832222, 126.952600479125);//서울대역
            

            // busan
            GeoCoordinate nampo = new GeoCoordinate(35.09796649185354, 129.0276002883911);
            GeoCoordinate sms = new GeoCoordinate(35.15533693760764, 129.058735370636);
            GeoCoordinate ssg = new GeoCoordinate(35.163047012439, 128.98273229599);
            GeoCoordinate jjd = new GeoCoordinate(35.23069588746653, 129.08681273460388);
            GeoCoordinate hdy = new GeoCoordinate(35.113194922077966, 128.96589875221252);
            GeoCoordinate dyd = new GeoCoordinate(35.13701929114497, 129.10048127174377);
            GeoCoordinate bjd = new GeoCoordinate(35.15518343040505, 129.06185746192932);
            GeoCoordinate dcs = new GeoCoordinate(35.2095682407033, 129.00581002235413);
            GeoCoordinate hud = new GeoCoordinate(35.16244181115321, 129.16219353675842);
            int zoom = 15;


            // 아이콘을 rectanglar로 지정
            Uri imgUri = new Uri("icons/blood-donate-icon.png", UriKind.RelativeOrAbsolute);
            BitmapImage imgSourceR = new BitmapImage(imgUri);
            ImageBrush imgBrush = new ImageBrush() { ImageSource = imgSourceR };


            // Create a pushpin to put at the center of the view
            Pushpin pin_scs = new Pushpin();
            pin_scs.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            pin_scs.Background = new SolidColorBrush(Colors.Transparent);
            pin_scs.Location = scs;
            pin_scs.Name = "신촌";
            pin_scs.Tag = pin_scs.Name;
            pin_scs.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            bloodMap.Children.Add(pin_scs);
            // Create a pushpin to put at the center of the view

            Pushpin pin_syf = new Pushpin();
            pin_syf.Location = syf;
            pin_syf.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            pin_syf.Name = "신촌연대";
            pin_syf.Tag = pin_syf.Name;
            pin_syf.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_syf.Background = new SolidColorBrush(Colors.Transparent);
            bloodMap.Children.Add(pin_syf);

            Pushpin pin_ssh = new Pushpin();
            pin_ssh.Location = ssh;
            pin_ssh.Name = "서울서부";
            pin_ssh.Tag = pin_ssh.Name;
            pin_ssh.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_ssh.Background = new SolidColorBrush(Colors.Transparent);
            pin_ssh.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_ssh);

            Pushpin pin_ujs = new Pushpin();
            pin_ujs.Location = ujs;
            pin_ujs.Name = "우장산역";
            pin_ujs.Tag = pin_ujs.Name;
            pin_ujs.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_ujs.Background = new SolidColorBrush(Colors.Transparent);
            pin_ujs.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_ujs);

            Pushpin pin_hd = new Pushpin();
            pin_hd.Location = hd;
            pin_hd.Name = "홍대";
            pin_hd.Tag = pin_hd.Name;
            pin_hd.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_hd.Background = new SolidColorBrush(Colors.Transparent);
            pin_hd.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_hd);


            Pushpin pin_ld = new Pushpin();
            pin_ld.Location = ld;
            pin_ld.Name = "이대";
            pin_ld.Tag = pin_ld.Name;
            pin_ld.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_ld.Background = new SolidColorBrush(Colors.Transparent);
            pin_ld.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_ld);

            Pushpin pin_sus = new Pushpin();
            pin_sus.Location = sus;
            pin_sus.Name = "서울역";
            pin_sus.Tag = pin_sus.Name;
            pin_sus.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_sus.Background = new SolidColorBrush(Colors.Transparent);
            pin_sus.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_sus);

            Pushpin pin_dbs = new Pushpin();
            pin_dbs.Location = dbs;
            pin_dbs.Name = "대방역";
            pin_dbs.Tag = pin_dbs.Name;
            pin_dbs.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_dbs.Background = new SolidColorBrush(Colors.Transparent);
            pin_dbs.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_dbs);

            Pushpin pin_stm = new Pushpin();
            pin_stm.Location = stm;
            pin_stm.Name = "신도림테크노마트";
            pin_stm.Tag = pin_stm.Name;
            pin_stm.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_stm.Background = new SolidColorBrush(Colors.Transparent);
            pin_stm.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_stm);

            Pushpin pin_gds = new Pushpin();
            pin_gds.Location = gds;
            pin_gds.Name = "구로디지털단지역";
            pin_gds.Tag = pin_gds.Name;
            pin_gds.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_gds.Background = new SolidColorBrush(Colors.Transparent);
            pin_gds.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_gds);

            Pushpin pin_sud = new Pushpin();
            pin_sud.Location = sud;
            pin_sud.Name = "서울대역";
            pin_sud.Tag = pin_sud.Name;
            pin_sud.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_sud.Background = new SolidColorBrush(Colors.Transparent);
            pin_sud.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_sud);

            Pushpin pin_isy = new Pushpin();
            pin_isy.Location = isy;
            pin_isy.Name = "이수";
            pin_isy.Tag = pin_isy.Name;
            pin_isy.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_isy.Background = new SolidColorBrush(Colors.Transparent);
            pin_isy.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_isy);

            Pushpin pin_gns = new Pushpin();
            pin_gns.Location = gns;
            pin_gns.Name = "강남";
            pin_gns.Tag = pin_gns.Name;
            pin_gns.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_gns.Background = new SolidColorBrush(Colors.Transparent);
            pin_gns.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_gns);

            Pushpin pin_gn2 = new Pushpin();
            pin_gn2.Location = gn2;
            pin_gn2.Name = "강남2";
            pin_gn2.Tag = pin_gn2.Name;
            pin_gn2.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_gn2.Background = new SolidColorBrush(Colors.Transparent);
            pin_gn2.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_gn2);

            Pushpin pin_gnd = new Pushpin();
            pin_gnd.Location = gnd;
            pin_gnd.Name = "강남면허";
            pin_gnd.Tag = pin_gnd.Name;
            pin_gnd.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_gnd.Background = new SolidColorBrush(Colors.Transparent);
            pin_gnd.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_gnd);

            Pushpin pin_nbh = new Pushpin();
            pin_nbh.Location = nbh;
            pin_nbh.Name = "서울남부";
            pin_nbh.Tag = pin_nbh.Name;
            pin_nbh.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_nbh.Background = new SolidColorBrush(Colors.Transparent);
            pin_nbh.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_nbh);

            Pushpin pin_chs = new Pushpin();
            pin_chs.Location = chs;
            pin_chs.Name = "천호";
            pin_chs.Tag = pin_chs.Name;
            pin_chs.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_chs.Background = new SolidColorBrush(Colors.Transparent);
            pin_chs.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_chs);

            Pushpin pin_jsy = new Pushpin();
            pin_jsy.Location = jsy;
            pin_jsy.Name = "잠실역";
            pin_jsy.Tag = pin_jsy.Name;
            pin_jsy.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_jsy.Background = new SolidColorBrush(Colors.Transparent);
            pin_jsy.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_jsy);

            Pushpin pin_cex = new Pushpin();
            pin_cex.Location = cex;
            pin_cex.Name = "코엑스";
            pin_cex.Tag = pin_cex.Name;
            pin_cex.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_cex.Background = new SolidColorBrush(Colors.Transparent);
            pin_cex.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_cex);

            Pushpin pin_dsu = new Pushpin();
            pin_dsu.Location = dsu;
            pin_dsu.Name = "동서울";
            pin_dsu.Tag = pin_dsu.Name;
            pin_dsu.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_dsu.Background = new SolidColorBrush(Colors.Transparent);
            pin_dsu.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_dsu);

            Pushpin pin_ds2 = new Pushpin();
            pin_ds2.Location = ds2;
            pin_ds2.Name = "동서울2";
            pin_ds2.Tag = pin_ds2.Name;
            pin_ds2.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_ds2.Background = new SolidColorBrush(Colors.Transparent);
            pin_ds2.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_ds2);

            Pushpin pin_kds = new Pushpin();
            pin_kds.Location = kds;
            pin_kds.Name = "건대역";
            pin_kds.Tag = pin_kds.Name;
            pin_kds.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_kds.Background = new SolidColorBrush(Colors.Transparent);
            pin_kds.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_kds);

            bloodMap.SetView(scs, zoom);

        }
        void pin1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var pp = sender as Pushpin;
            int k = String.Compare(pp.Name, pp.Tag.ToString());
                        if (k==0)
                        {
                            pp.Tag = "";
                        }
                        else
                        {
                            pp.Tag = pp.Name;
                        }
                        MessageBox.Show(pp.Name + "\n" + bh[pp.Name]);
        }
         void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
            {
                //throw new NotImplementedException();
                // non-UI thread 에서 실행되므로 UI thread 와의 연동을 위해 Dispatcher를 사용
                Deployment.Current.Dispatcher.BeginInvoke(() => MyPositionChanged(e));
            }
     
            void MyPositionChanged(GeoPositionChangedEventArgs<GeoCoordinate> e)
            {
                // throw new NotImplementedException();
                Double a = e.Position.Location.Latitude;
                Double b = e.Position.Location.Longitude;

                GeoCoordinate mapCenter = new GeoCoordinate(a, b);
                bloodMap.SetView(mapCenter, 14);
                //MessageBox.Show("My lat long is:"+a+", "+b);
                watcher.Stop(); // for battery issue
            }
            private void hereBt_Click(object sender, EventArgs e)
            {
                watcher.Start();
                //Do work for your application here.
            }

            private void westBt_Click(object sender, EventArgs e)
            {
                GeoCoordinate hd = new GeoCoordinate(37.5558491, 126.9228391);//홍대
                bloodMap.SetView(hd, 13);
                //Do work for your application here.
            }
            private void southBt_Click(object sender, EventArgs e)
            {
                GeoCoordinate gns = new GeoCoordinate(37.50122322, 127.025599479675);//강남
                bloodMap.SetView(gns, 13);
                //Do work for your application here.
            }
            private void infoBt_Click(object sender, EventArgs e)
            {
                //Do work for your application here.
                MessageBox.Show("헌혈의집 위치 및 운영안내\n업데이트 문의:lispro06@gmail.com");
            }
    }
}