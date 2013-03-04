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
	        {"서울동부", "평일 : 09:00~18:00 \n전화번호 : 02-952-0322\n서울 노원구 상계6동 764\n(중계역6번출구 노원역방향 150m)"},
	        {"광화문", "평일 : 09:00~18:00 \n전화번호 : 02-732-1027\n서울 종로구 종로1가 르메이에르종로타운1 2층204호"},
	        {"명동", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 02-777-1291\n서울 중구 명동2가 31-5 타워빌딩 4층\n(4호선 명동역 8번출구)"},
	        {"종로", "평일 : 09:00~18:00 \n전화번호 : 02-762-1978\n서울 종로구 종로3가 10 낙원빌딩 6층\n(종로3가역 1번 출구)"},
	        {"동대문", "평일 : 09:00~18:00 \n전화번호 : 02-2272-1370\n서울시 중구 을지로 6가 18-95 (밀리오레 옆) 5층\n동대문운동장역14번출구"},
	        {"대학로", "평일 : 09:00~18:00 \n전화번호 : 02-743-1972\n서울 종로구 명륜동2가 36-1\n(4호선 혜화역 4번 출구)"},
	        {"돈암", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 02-925-3566\n서울 성북구 동선동1가 3-2 천일빌딩 4층\n(성신여대역 1번출구 200m GS왓슨스 4층)"},
	        {"고려대앞", "평일 : 09:00~18:00 \n전화번호 : 02-967-3852\n서울 성북구 안암동5가 102-50\n(안암역 3번출구 안경박사 2층)"},
	        {"한양대앞", "평일 : 09:00~18:00 \n전화번호 : 02-2296-1076\n서울 성동구 행당동 산17번지\n(2호선 한양대역내-2번 출구 방면)"},
	        {"수유", "평일 : 09:00~18:00 \n전화번호 : 02-900-4772\n서울 강북구 번동 446-46 2층\n(수유역 3번출구)"},
	        {"회기", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 02-969-6199\n서울 동대문구 휘경동 319-13 두리빌딩 5층\n(회기역 1번 출구)"},
	        {"노원", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 02-934-5340\n서울 노원구 상계동 598-2번지 7층\n(노원역2번 출구, 화랑화장품빌딩 7층)"},
	        {"도봉면허시험장", "평일 : 09:00~18:00 \n전화번호 : 02-935-0322\n서울 노원구 동1로 727\n(도봉면허시험장 내)"},
	        {"의정부", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 031-841-0322\n경기 의정부시 의정부동 195-1 2층\n(1층스무디킹,신한은행건너편)"},
	        {"구리", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 031-563-5322\n경기 구리시 인창동 280-4 명동빌딩 3층"},
            {"서울서부", "평일 : 09:00~18:00 \n전화번호 : 02-952-0322\n서울 노원구 상계6동 764\n(중계역6번출구 노원역방향 150m)"},
            {"일산", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 031-901-5492\n경기 고양시 일산동구 장항동효산캐슬 2층\n(정발산역1번출구 벧엘교회 방향 카페베네 2층)"},
            {"연신내", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 02-353-7750\n서울 은평구 갈현동 396-12 와이타운4층\n(연신내역 6번출구)"},
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
	        {"서울대학교", "평일 : 09:00-18:00 \n전화번호 : 02-886-2479\n서울 관악구 대학동\n서울대학교 두레문예관 내"},
	        {"서울남부", "평일 : 09:00-18:00 \n전화번호 : 02-570-0662"},
	        {"잠실역", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 02-2202-7479"},
	        {"이수", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 02-578-9811"},
	        {"강남", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 02-533-0770"},
	        {"강남2", "평일 : 09:00-18:00 \n전화번호 : 02-564-1525"},
	        {"강남면허", "평일 : 09:00-18:00 \n전화번호 : 02-565-8332"},
	        {"코엑스", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 02-551-0600"},
	        {"천호", "평,토(일) : 09:00(10:00)~18:00 \n전화번호 : 02-485-3515"},
	        {"동서울", "평일 : 09:00-18:00 \n전화번호 : 02-2201-8481"},
	        {"동서울2", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 02-446-3526"},
	        {"건대역", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 02-498-4185\n서울 광진구 화양동 5-91 하마빌딩 4층\n(건대역 2번출구 건대맛거리입구 50m이내)"},
            {"노량진역", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 02-825-2916\n서울 동작구 노량진동 73-17 동작경찰서 옆\n(1호선9호선 노량진역 3번출구)"}
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

                 ApplicationBarMenuItem eastBt = new ApplicationBarMenuItem("서울동부");
                 ApplicationBar.MenuItems.Add(eastBt);

                 eastBt.Click += new EventHandler(eastBt_Click);

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
                //서울동부
                GeoCoordinate dbh = new GeoCoordinate(37.6470773, 127.0621925);//서울동부
                GeoCoordinate ghm = new GeoCoordinate(37.570471390446, 126.978403329849);//광화문
                GeoCoordinate mds = new GeoCoordinate(37.561716297985384, 126.98564529418945);//명동
                GeoCoordinate jns = new GeoCoordinate(37.5705649312881, 126.990355253219);//종로
                GeoCoordinate ddm = new GeoCoordinate(37.5677573, 127.0081815);//동대문
                GeoCoordinate dhr = new GeoCoordinate(37.5834462, 127.0002543);//대학로
                GeoCoordinate dad = new GeoCoordinate(37.5919779, 127.0177036);//돈암
                GeoCoordinate kor = new GeoCoordinate(37.585719, 127.0295347);//고려대앞
                GeoCoordinate hyd = new GeoCoordinate(37.555592649547606, 127.04383850097656);//한양대앞
                GeoCoordinate sys = new GeoCoordinate(37.63706821603881, 127.02552437782288);//수유
                GeoCoordinate hgs = new GeoCoordinate(37.58939401090336, 127.05671310424805);//회기
                GeoCoordinate nws = new GeoCoordinate(37.655948418327085, 127.06271052360535);//노원
                GeoCoordinate dbc = new GeoCoordinate(37.6578723, 127.0579876);//도봉면허시험장
                GeoCoordinate ujb = new GeoCoordinate(37.7397129, 127.0482332);//의정부
                GeoCoordinate grc = new GeoCoordinate(37.6009377, 127.1400906);//구리

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
                GeoCoordinate nrj = new GeoCoordinate(37.5134534141747, 126.942461729049);//노량진역

                //서울서부
                GeoCoordinate ssh = new GeoCoordinate(37.5481129, 126.8708138);//서울서부혈액원
                GeoCoordinate ils = new GeoCoordinate(37.657575, 126.770894);//일산
                GeoCoordinate ysn = new GeoCoordinate(37.6188647, 126.9204747);//연신내
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
                GeoCoordinate snu = new GeoCoordinate(37.4635987, 126.9496771);//서울대역
            

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
            Pushpin pin_dbh = new Pushpin();
            pin_dbh.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            pin_dbh.Background = new SolidColorBrush(Colors.Transparent);
            pin_dbh.Location = dbh;
            pin_dbh.Name = "서울동부";
            pin_dbh.Tag = pin_dbh.Name;
            pin_dbh.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            bloodMap.Children.Add(pin_dbh);

            // Create a pushpin to put at the center of the view
            Pushpin pin_ghm = new Pushpin();
            pin_ghm.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            pin_ghm.Background = new SolidColorBrush(Colors.Transparent);
            pin_ghm.Location = ghm;
            pin_ghm.Name = "광화문";
            pin_ghm.Tag = pin_ghm.Name;
            pin_ghm.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            bloodMap.Children.Add(pin_ghm);

            // Create a pushpin to put at the center of the view
            Pushpin pin_mds = new Pushpin();
            pin_mds.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            pin_mds.Background = new SolidColorBrush(Colors.Transparent);
            pin_mds.Location = mds;
            pin_mds.Name = "명동";
            pin_mds.Tag = pin_mds.Name;
            pin_mds.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            bloodMap.Children.Add(pin_mds);
            // Create a pushpin to put at the center of the view

            Pushpin pin_jns = new Pushpin();
            pin_jns.Location = jns;
            pin_jns.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            pin_jns.Name = "종로";
            pin_jns.Tag = pin_jns.Name;
            pin_jns.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_jns.Background = new SolidColorBrush(Colors.Transparent);
            bloodMap.Children.Add(pin_jns);

            Pushpin pin_ddm = new Pushpin();
            pin_ddm.Location = ddm;
            pin_ddm.Name = "동대문";
            pin_ddm.Tag = pin_ddm.Name;
            pin_ddm.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_ddm.Background = new SolidColorBrush(Colors.Transparent);
            pin_ddm.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_ddm);

            Pushpin pin_dhr = new Pushpin();
            pin_dhr.Location = dhr;
            pin_dhr.Name = "대학로";
            pin_dhr.Tag = pin_dhr.Name;
            pin_dhr.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_dhr.Background = new SolidColorBrush(Colors.Transparent);
            pin_dhr.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_dhr);

            Pushpin pin_dad = new Pushpin();
            pin_dad.Location = dad;
            pin_dad.Name = "돈암";
            pin_dad.Tag = pin_dad.Name;
            pin_dad.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_dad.Background = new SolidColorBrush(Colors.Transparent);
            pin_dad.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_dad);


            Pushpin pin_kor = new Pushpin();
            pin_kor.Location = kor;
            pin_kor.Name = "고려대앞";
            pin_kor.Tag = pin_kor.Name;
            pin_kor.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_kor.Background = new SolidColorBrush(Colors.Transparent);
            pin_kor.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_kor);

            Pushpin pin_hyd = new Pushpin();
            pin_hyd.Location = hyd;
            pin_hyd.Name = "한양대앞";
            pin_hyd.Tag = pin_hyd.Name;
            pin_hyd.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_hyd.Background = new SolidColorBrush(Colors.Transparent);
            pin_hyd.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_hyd);

            Pushpin pin_sys = new Pushpin();
            pin_sys.Location = sys;
            pin_sys.Name = "수유";
            pin_sys.Tag = pin_sys.Name;
            pin_sys.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_sys.Background = new SolidColorBrush(Colors.Transparent);
            pin_sys.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_sys);

            Pushpin pin_hgs = new Pushpin();
            pin_hgs.Location = hgs;
            pin_hgs.Name = "회기";
            pin_hgs.Tag = pin_hgs.Name;
            pin_hgs.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_hgs.Background = new SolidColorBrush(Colors.Transparent);
            pin_hgs.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_hgs);

            Pushpin pin_nws = new Pushpin();
            pin_nws.Location = nws;
            pin_nws.Name = "노원";
            pin_nws.Tag = pin_nws.Name;
            pin_nws.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_nws.Background = new SolidColorBrush(Colors.Transparent);
            pin_nws.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_nws);

            Pushpin pin_dbc = new Pushpin();
            pin_dbc.Location = dbc;
            pin_dbc.Name = "도봉면허시험장";
            pin_dbc.Tag = pin_dbc.Name;
            pin_dbc.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_dbc.Background = new SolidColorBrush(Colors.Transparent);
            pin_dbc.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_dbc);

            Pushpin pin_ujb = new Pushpin();
            pin_ujb.Location = ujb;
            pin_ujb.Name = "의정부";
            pin_ujb.Tag = pin_ujb.Name;
            pin_ujb.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_ujb.Background = new SolidColorBrush(Colors.Transparent);
            pin_ujb.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_ujb);


            Pushpin pin_grc = new Pushpin();
            pin_grc.Location = grc;
            pin_grc.Name = "의정부";
            pin_grc.Tag = pin_grc.Name;
            pin_grc.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_grc.Background = new SolidColorBrush(Colors.Transparent);
            pin_grc.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_grc);

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
            Pushpin pin_ils = new Pushpin();
            pin_ils.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            pin_ils.Background = new SolidColorBrush(Colors.Transparent);
            pin_ils.Location = ils;
            pin_ils.Name = "일산";
            pin_ils.Tag = pin_scs.Name;
            pin_ils.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            bloodMap.Children.Add(pin_ils);

            // Create a pushpin to put at the center of the view
            Pushpin pin_ysn = new Pushpin();
            pin_ysn.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            pin_ysn.Background = new SolidColorBrush(Colors.Transparent);
            pin_ysn.Location = ysn;
            pin_ysn.Name = "연신내";
            pin_ysn.Tag = pin_scs.Name;
            pin_ysn.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            bloodMap.Children.Add(pin_ysn);
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

            Pushpin pin_snu = new Pushpin();
            pin_snu.Location = snu;
            pin_snu.Name = "서울대학교";
            pin_snu.Tag = pin_snu.Name;
            pin_snu.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_snu.Background = new SolidColorBrush(Colors.Transparent);
            pin_snu.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_snu);

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

            Pushpin pin_nrj = new Pushpin();
            pin_nrj.Location = nrj;
            pin_nrj.Name = "노량진역";
            pin_nrj.Tag = pin_nrj.Name;
            pin_nrj.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_nrj.Background = new SolidColorBrush(Colors.Transparent);
            pin_nrj.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_nrj);



            bloodMap.SetView(scs, zoom);//지도 보이기

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
                bloodMap.SetView(mapCenter, 13);
                //MessageBox.Show("My lat long is:"+a+", "+b);
                watcher.Stop(); // for battery issue
            }
            private void hereBt_Click(object sender, EventArgs e)
            {
                watcher.Start();
                //Do work for your application here.
            }

            private void eastBt_Click(object sender, EventArgs e)
            {
                GeoCoordinate dbh = new GeoCoordinate(37.6470773, 127.0621925);//서울동부
                bloodMap.SetView(dbh, 12);
                //Do work for your application here.
            }
            private void westBt_Click(object sender, EventArgs e)
            {
                GeoCoordinate ssh = new GeoCoordinate(37.5481129, 126.8708138);//서울서부혈액원
                bloodMap.SetView(ssh, 12);
                //Do work for your application here.
            }
            private void southBt_Click(object sender, EventArgs e)
            {
                GeoCoordinate jsy = new GeoCoordinate(37.514066147794594, 127.10201025009155);//잠실역
                bloodMap.SetView(jsy, 12);
                //Do work for your application here.
            }
            private void infoBt_Click(object sender, EventArgs e)
            {
                //Do work for your application here.
                MessageBox.Show("헌혈의집 위치 및 운영안내\n업데이트 문의:lispro06@gmail.com");
            }
    }
}