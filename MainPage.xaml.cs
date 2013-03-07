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
            {"서울서부", "평일 : 09:00~18:00 \n전화번호 : 02-952-0322\n서울 강서구 염창동 280-17 (강서보건소와 서울도시가스 중간에 위치)"},
            {"일산", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 031-901-5492\n경기 고양시 일산동구 장항동효산캐슬 2층\n(정발산역1번출구 벧엘교회 방향 카페베네 2층)"},
            {"연신내", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 02-353-7750\n서울 은평구 갈현동 396-12 와이타운4층\n(연신내역 6번출구)"},
	        {"우장산역", "평일 : 09:00-18:00 \n전화번호 : 02-2603-5817\n서울 강서구 화곡동 1006-9 (지하철5호선 우장산역 3번 출구 150m)"},
	        {"홍대", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 02-323-5420\n서울 마포구 동교동 162-1 대화빌딩 6층 (홍대입구역 9번출구)"},
	        {"이대", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 02-715-3105\n서울 서대문구 대현동 40-4 (이대입구역 2번출구)"},
	        {"신촌", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 02-392-6460\n서울 서대문구 창천동 18-5번지 8층 (2호선 신촌역 3번 출구 신촌빌딩 8층)"},
	        {"신촌연대", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 02-312-1247\n서울 서대문구 창천동 18-5번지 8층 (2호선 신촌역 3번 출구 신촌빌딩 8층)"},
	        {"서울역", "평일 : 09:00-18:00 \n전화번호 : 02-752-9020\n서울 중구 봉래동2가 122 (1호선 서울역 2번 출구)"},
	        {"대방역", "평일 : 09:00-18:00 \n전화번호 : 02-825-6560\n서울 영등포구 신길7동 1372(대방역 5번 출구)"},
	        {"구로디지털단지역", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 02-869-9415\n서울 구로구 구로3동 1120 (지하철 2호선 구로디지털단지역 1번 출구 오른쪽 방향)"},
	        {"신도림테크노마트", "평일 : 09:00-18:00 \n전화번호 : 02-861-0801\n서울 구로구 구로동 3-33 지하광장(테크노마트)"},
	        {"서울대역", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 02-873-4364\n서울 관악구 봉천7동 1598-18(지하철2호선 서울대입구역 2번출구 관악구청 맞은편 2층)"},
	        {"서울대학교", "평일 : 09:00-18:00 \n전화번호 : 02-886-2479\n서울 관악구 대학동\n서울대학교 두레문예관 내"},
	        {"서울남부", "평일 : 09:00-18:00 \n전화번호 : 02-570-0662\n서울 강남구 개포동 1267 (매봉역 4번출구 양재천쪽 10분 도보)"},
	        {"잠실역", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 02-2202-7479\n서울 송파구 잠실동 40-1 (잠실역3번출구 롯데백화점앞 지하광장 롯데리아 옆)"},
	        {"이수", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 02-578-9811\n서울 동작구 동작대로 109 경문빌딩 3층"},
	        {"강남", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 02-533-0770\n서울 서초구 서초4동 1301∼1306 1305-3 대원빌딩7층(2호선 강남역 10번출구 CGV극장건너편)"},
	        {"강남2", "평일 : 09:00-18:00 \n전화번호 : 02-564-1525\n서울 강남구 역삼동825-9 준빌딩9층 (2호선 강남역 2번출구)"},
	        {"강남면허", "평일 : 09:00-18:00 \n전화번호 : 02-565-8332\n서울 강남구 대치동 999-5 강남면허시험장앞 두원빌딩 1층 (2호선 삼성역 1번출구)"},
	        {"코엑스", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 02-551-0600\n서울 강남구 삼성동159번지 코엑스몰(R7) 강변길 토다이 옆, 2호선 삼성역 5번출구"},
	        {"천호", "평,토(일) : 09:00(10:00)~18:00 \n전화번호 : 02-485-3515\n서울 강동구 천호동 453-15호 강동빌딩 6층 (천호역 5번출구)"},
	        {"동서울", "평일 : 09:00-18:00 \n전화번호 : 02-2201-8481\n서울 광진구 구의동 546-1번지 동서울터미널앞 컨테이너건물"},
	        {"동서울2", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 02-446-3526\n서울 광진구 구의동 546-1 동서울터미널내 1층 114호 (2호선 강변역 4번출구)"},
	        {"건대역", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 02-498-4185\n서울 광진구 화양동 5-91 하마빌딩 4층\n(건대역 2번출구 건대맛거리입구 50m이내)"},
            {"노량진역", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 02-825-2916\n서울 동작구 노량진동 73-17 동작경찰서 옆\n(1호선9호선 노량진역 3번출구)"},
            {"야탑", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 031-707-3791\n경기 성남시 분당구 야탑동 353-5\n(분당종합상가 3층, 분당선 야탑역 4번출구)"},
            {"서현", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 031-707-3795\n경기 성남시 분당구 서현동 269-5번지 서원플라자 4층\n(분당선 서현역 AK플라자 6번출구)"},
            {"아주대", "평일 : 09:00~18:00 \n전화번호 :031-214-8550\n경기 수원시 팔달구 우만2동 578-1호 용성빌딩 2층\n(아주대삼거리에서 아주대방향 좌측)"},
            {"경기", "평일 : 09:00~18:00 \n전화번호 : 031-220-8518\n경기도 수원시 권선구 권선동 1015-6\n(농협사거리에서 이비스앰버서더호텔 맞은편)"},
            {"수원역", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 031-245-6518\n경기 수원시 팔달구 매산로1가 57-104호 새수원빌딩 4층\n(애경백화점 육교건너편 로타리 택시승차장앞 건물)"},
            {"한대앞역", "평일 : 10:00~19:00 \n전화번호 : 031-406-5031\n경기 안산시 상록구 이동 19번지\n(한대앞역 2번 출구, 공영주차장 내)"},
            {"산본", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 031-397-7600\n경기 군포시 산본동 1134번지 센타빌딩 3층"},
            {"평촌", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 031-387-8842\n경기 안양시 동안구 호계동 1039-2\n(롯데백화점 옆건물, 범계역 4-1번출구)"},
            {"안양", "평일 : 09:00~18:00 \n전화번호 : 031-707-3795\n경기 안양시 만안구 안양1동 674-19번지 3층\n(안양역사 맞은편 지하상가 5번 출구)"},
            {"평택역", "평일 : 10:00~19:00 \n전화번호 : 031-656-8844\n경기 평택시 평택동 55-7 아케이트상가 가동 1층 9-2호\n(평택역 우측 맞은편)"},
	        {"인천", "평일 : 09:00~18:00 \n전화번호 : 032-815-0631\n인천 연수구 연수3동 581(신연수역 3번출구 도보5분)"},
	        {"주안", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 032-428-5195\n인천 남구 주안동 173-1 필프라자 201(주안역 시민회관방향 작은사거리 건너 2층)"},
	        {"구월", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 032-421-9013\n인천 남동구 구월동 1468-1 신현프라자 201호(인천터미널역 2번출구)"},
	        {"부평", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 032-504-0975\n인천 부평구 부평1동 738-21 부평역사1층 이벤트광장 옆"},
	        {"상동", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 032-328-3052\n경기 부천시 원미구 상동544-4 가나베스트타운 205호(세이브존 건너편 롯데리아 2층)"},
	        {"부천", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 032-651-1618\n경기 부천시 원미구 심곡동 172-15 경동빌딩 3층(부천역 5번 출구)"},
	        {"광명", "평일 : 10:00~19:00 \n전화번호 : 02-2060-5473\n경기 광명시 광명3동 158-81 현대아이타워 5층 502호(7호선 광명사거리역 8번출구)"},
	        {"덕천", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 051-335-7505\n부산 북구 덕천동 403-2번지 (덕천로타리 무궁화마트 3층)"},
	        {"서면", "평,토(일) : 10:00~20:00(18:30) \n전화번호 : 051-809-7505\n부산 부산진구 부전동 191-6(서면 쥬디스태화 옆 커피빈 건물 2층)"},
	        {"부전", "평,토(일) : 10:00~19:00(18:00) \n전화번호 : 051-804-7505\n부산 부산진구 부전2동 169-2 부전시립도서관 옆 건물 2층"},
	        {"대연", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 032-504-0975\n부산 남구 대연동 72-2(동아서적 건물 5층)"},
	        {"광복", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 051-245-7505\n부산 중구 중앙동1가 26번지 광복지하도상가 A-13, 14"},
	        {"남포", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 051-246-7505\n부산 중구 남포동5가 3-7(피프광장에서 중앙동쪽으로 피닉스 호텔 옆 약5m)"},
	        {"하단", "평일 : 10:00~19:00 \n전화번호 : 051-204-1691\n부산 사하구 하단동 494-3 에덴상가 13-14 1층"},
	        {"동의대", "평일 : 10:00~19:00 \n전화번호 : 051-892-2505\n부산 부산진구 가야2동 동의대학교 생활과학대학 112호"},
	        {"부산", "평일 : 09:00~18:00 \n전화번호 : 051-816-9151\n부산 부산진구 전포3동 362-5 (지하철 전포역 2번출구 혈액원 4층)"},
		    {"해운대", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 051-746-9505\n부산 해운대구 중동 1394-385 혜천빌딩 2층(세이븐존 정문 앞)"},
	        {"부산대학로", "평,토 : 10:00~19:00 \n전화번호 : 051-313-7505\n부산 금정구 장전동 400-61번지(부산대학교 정문 앞 건물 3~4층)"},
	        {"장전동", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 051-892-2505\n부산 금정구 장전3동 30-17 (지하철 부산대학역 앞 스타벅스 건물 옆 2층)"},
	        {"동의과학대학", "평일 : 10:00~19:00 \n전화번호 : 051-861-5505\n부산 부산진구 가야3동 산 72번지 동의과학대학 진리관"}
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

                 ApplicationBarMenuItem ichBt = new ApplicationBarMenuItem("인천지역");
                 ApplicationBar.MenuItems.Add(ichBt);

                 ichBt.Click += new EventHandler(ichBt_Click);


                 ApplicationBarMenuItem gghBt = new ApplicationBarMenuItem("경기지역");
                 ApplicationBar.MenuItems.Add(gghBt);

                 gghBt.Click += new EventHandler(gghBt_Click);


                 ApplicationBarMenuItem bshBt = new ApplicationBarMenuItem("부산지역");
                 ApplicationBar.MenuItems.Add(bshBt);

                 bshBt.Click += new EventHandler(bshBt_Click);


                 ApplicationBarMenuItem infoBt = new ApplicationBarMenuItem("앱 정보");
                 ApplicationBar.MenuItems.Add(infoBt);

                 infoBt.Click += new EventHandler(infoBt_Click);

             // GPS init
                watcher = new GeoCoordinateWatcher();
                watcher.MovementThreshold = 20;
                watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(watcher_PositionChanged);
                // Set the center coordinate and zoom level

                GeoCoordinate seoul = new GeoCoordinate(37.5502029, 126.9903724);//남산

                //인천
                GeoCoordinate ich = new GeoCoordinate(37.4197868, 126.6898464);//인천
                GeoCoordinate jas = new GeoCoordinate(37.4608639, 126.6807963);//주안
                GeoCoordinate bsd = new GeoCoordinate(37.50524, 126.7524429);//상동
                GeoCoordinate bps = new GeoCoordinate(37.489689, 126.7226808);//부평
                GeoCoordinate gwd = new GeoCoordinate(37.4439905, 126.70159);//구월
                GeoCoordinate bcs = new GeoCoordinate(37.4844107, 126.7835564);//부천
                GeoCoordinate gms = new GeoCoordinate(37.4797507, 126.8549005);//광명

                //경기
                GeoCoordinate yts = new GeoCoordinate(37.4116124, 127.1276236);//야탑
                GeoCoordinate shs = new GeoCoordinate(37.384074, 127.1215402);//서현
                GeoCoordinate aju = new GeoCoordinate(37.2757817, 127.0436672);//아주대
                GeoCoordinate ggh = new GeoCoordinate(37.2596911, 127.03058);//경기
                GeoCoordinate sws = new GeoCoordinate(37.2661541, 127.001673);//수원역
                GeoCoordinate hus = new GeoCoordinate(37.3111104, 126.8635734);//한대앞역
                GeoCoordinate sbs = new GeoCoordinate(37.3596826, 126.9320291);//산본역
                GeoCoordinate pcs = new GeoCoordinate(37.3897693, 126.9510733);//평촌역
                GeoCoordinate ays = new GeoCoordinate(37.4006057, 126.9219608);//안양
                GeoCoordinate pts = new GeoCoordinate(36.9917181, 127.0862661);//평택역

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
                GeoCoordinate dcs = new GeoCoordinate(35.2095682407033, 129.00581002235413);//덕천
                GeoCoordinate sms = new GeoCoordinate(35.15533693760764, 129.058735370636);//서면
                GeoCoordinate bjd = new GeoCoordinate(35.15518343040505, 129.06185746192932);//부전
                GeoCoordinate dyd = new GeoCoordinate(35.13701929114497, 129.10048127174377);//대연
                GeoCoordinate gbd = new GeoCoordinate(35.100817, 129.0357538);//광복
                GeoCoordinate npd = new GeoCoordinate(35.09796649185354, 129.0276002883911);//남포
                GeoCoordinate hdy = new GeoCoordinate(35.113194922077966, 128.96589875221252);//하단
                GeoCoordinate dud = new GeoCoordinate(35.1426019, 129.033937);//동의대
                GeoCoordinate bsh = new GeoCoordinate(35.1515078, 129.0658822);//부산
                GeoCoordinate hud = new GeoCoordinate(35.16244181115321, 129.16219353675842);//해운대
                GeoCoordinate bsu = new GeoCoordinate(35.231358, 129.0842462);//부산대학로
                GeoCoordinate jjd = new GeoCoordinate(35.23069588746653, 129.08681273460388);//장전동
                GeoCoordinate dgu = new GeoCoordinate(35.1658384, 129.0722408);//동의과학대학
            int zoom = 12;


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
            pin_grc.Name = "구리";
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


            Pushpin pin_ggh = new Pushpin();
            pin_ggh.Location = ggh;
            pin_ggh.Name = "경기";
            pin_ggh.Tag = pin_ggh.Name;
            pin_ggh.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_ggh.Background = new SolidColorBrush(Colors.Transparent);
            pin_ggh.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_ggh);

            Pushpin pin_yts = new Pushpin();
            pin_yts.Location = yts;
            pin_yts.Name = "야탑";
            pin_yts.Tag = pin_yts.Name;
            pin_yts.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_yts.Background = new SolidColorBrush(Colors.Transparent);
            pin_yts.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_yts);

            Pushpin pin_shs = new Pushpin();
            pin_shs.Location = shs;
            pin_shs.Name = "서현";
            pin_shs.Tag = pin_shs.Name;
            pin_shs.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_shs.Background = new SolidColorBrush(Colors.Transparent);
            pin_shs.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_shs);

            Pushpin pin_aju = new Pushpin();
            pin_aju.Location = aju;
            pin_aju.Name = "아주대";
            pin_aju.Tag = pin_aju.Name;
            pin_aju.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_aju.Background = new SolidColorBrush(Colors.Transparent);
            pin_aju.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_aju);

            Pushpin pin_sws = new Pushpin();
            pin_sws.Location = sws;
            pin_sws.Name = "수원역";
            pin_sws.Tag = pin_sws.Name;
            pin_sws.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_sws.Background = new SolidColorBrush(Colors.Transparent);
            pin_sws.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_sws);

            Pushpin pin_hus = new Pushpin();
            pin_hus.Location = hus;
            pin_hus.Name = "한대앞역";
            pin_hus.Tag = pin_hus.Name;
            pin_hus.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_hus.Background = new SolidColorBrush(Colors.Transparent);
            pin_hus.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_hus);

            Pushpin pin_sbs = new Pushpin();
            pin_sbs.Location = sbs;
            pin_sbs.Name = "산본";
            pin_sbs.Tag = pin_sbs.Name;
            pin_sbs.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_sbs.Background = new SolidColorBrush(Colors.Transparent);
            pin_sbs.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_sbs);

            Pushpin pin_pcs = new Pushpin();
            pin_pcs.Location = pcs;
            pin_pcs.Name = "평촌";
            pin_pcs.Tag = pin_pcs.Name;
            pin_pcs.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_pcs.Background = new SolidColorBrush(Colors.Transparent);
            pin_pcs.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_pcs);

            Pushpin pin_ays = new Pushpin();
            pin_ays.Location = ays;
            pin_ays.Name = "안양";
            pin_ays.Tag = pin_ays.Name;
            pin_ays.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_ays.Background = new SolidColorBrush(Colors.Transparent);
            pin_ays.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_ays);

            Pushpin pin_pts = new Pushpin();
            pin_pts.Location = pts;
            pin_pts.Name = "평택역";
            pin_pts.Tag = pin_pts.Name;
            pin_pts.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_pts.Background = new SolidColorBrush(Colors.Transparent);
            pin_pts.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_pts);

            // Create a pushpin to put at the center of the view
            Pushpin pin_ich = new Pushpin();
            pin_ich.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            pin_ich.Background = new SolidColorBrush(Colors.Transparent);
            pin_ich.Location = ich;
            pin_ich.Name = "인천";
            pin_ich.Tag = pin_ich.Name;
            pin_ich.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            bloodMap.Children.Add(pin_ich);

            // Create a pushpin to put at the center of the view
            Pushpin pin_jas = new Pushpin();
            pin_jas.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            pin_jas.Background = new SolidColorBrush(Colors.Transparent);
            pin_jas.Location = jas;
            pin_jas.Name = "주안";
            pin_jas.Tag = pin_jas.Name;
            pin_jas.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            bloodMap.Children.Add(pin_jas);

            // Create a pushpin to put at the center of the view
            Pushpin pin_bsd = new Pushpin();
            pin_bsd.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            pin_bsd.Background = new SolidColorBrush(Colors.Transparent);
            pin_bsd.Location = bsd;
            pin_bsd.Name = "상동";
            pin_bsd.Tag = pin_bsd.Name;
            pin_bsd.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            bloodMap.Children.Add(pin_bsd);
            // Create a pushpin to put at the center of the view

            Pushpin pin_gwd = new Pushpin();
            pin_gwd.Location = gwd;
            pin_gwd.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            pin_gwd.Name = "구월";
            pin_gwd.Tag = pin_gwd.Name;
            pin_gwd.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_gwd.Background = new SolidColorBrush(Colors.Transparent);
            bloodMap.Children.Add(pin_gwd);

            Pushpin pin_bps = new Pushpin();
            pin_bps.Location = bps;
            pin_bps.Name = "부평";
            pin_bps.Tag = pin_bps.Name;
            pin_bps.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_bps.Background = new SolidColorBrush(Colors.Transparent);
            pin_bps.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_bps);

            Pushpin pin_bcs = new Pushpin();
            pin_bcs.Location = bcs;
            pin_bcs.Name = "부천";
            pin_bcs.Tag = pin_bcs.Name;
            pin_bcs.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_bcs.Background = new SolidColorBrush(Colors.Transparent);
            pin_bcs.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_bcs); 

            Pushpin pin_gms = new Pushpin();
            pin_gms.Location = gms;
            pin_gms.Name = "광명";
            pin_gms.Tag = pin_gms.Name;
            pin_gms.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_gms.Background = new SolidColorBrush(Colors.Transparent);
            pin_gms.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_gms);
            // Create a pushpin to put at the center of the view
            Pushpin pin_dcs = new Pushpin();
            pin_dcs.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            pin_dcs.Background = new SolidColorBrush(Colors.Transparent);
            pin_dcs.Location = dcs;
            pin_dcs.Name = "덕천";
            pin_dcs.Tag = pin_dcs.Name;
            pin_dcs.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            bloodMap.Children.Add(pin_dcs);

            // Create a pushpin to put at the center of the view
            Pushpin pin_sms = new Pushpin();
            pin_sms.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            pin_sms.Background = new SolidColorBrush(Colors.Transparent);
            pin_sms.Location = sms;
            pin_sms.Name = "서면";
            pin_sms.Tag = pin_sms.Name;
            pin_sms.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            bloodMap.Children.Add(pin_sms);

            // Create a pushpin to put at the center of the view
            Pushpin pin_bjd = new Pushpin();
            pin_bjd.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            pin_bjd.Background = new SolidColorBrush(Colors.Transparent);
            pin_bjd.Location = bjd;
            pin_bjd.Name = "부전";
            pin_bjd.Tag = pin_bjd.Name;
            pin_bjd.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            bloodMap.Children.Add(pin_bjd);
            // Create a pushpin to put at the center of the view

            Pushpin pin_dyd = new Pushpin();
            pin_dyd.Location = dyd;
            pin_dyd.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            pin_dyd.Name = "대연";
            pin_dyd.Tag = pin_dyd.Name;
            pin_dyd.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_dyd.Background = new SolidColorBrush(Colors.Transparent);
            bloodMap.Children.Add(pin_dyd);

            Pushpin pin_gbd = new Pushpin();
            pin_gbd.Location = gbd;
            pin_gbd.Name = "광복";
            pin_gbd.Tag = pin_gbd.Name;
            pin_gbd.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_gbd.Background = new SolidColorBrush(Colors.Transparent);
            pin_gbd.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_gbd);


            Pushpin pin_npd = new Pushpin();
            pin_npd.Location = npd;
            pin_npd.Name = "남포";
            pin_npd.Tag = pin_npd.Name;
            pin_npd.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_npd.Background = new SolidColorBrush(Colors.Transparent);
            pin_npd.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_npd);


            Pushpin pin_hdy = new Pushpin();
            pin_hdy.Location = hdy;
            pin_hdy.Name = "하단";
            pin_hdy.Tag = pin_hdy.Name;
            pin_hdy.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_hdy.Background = new SolidColorBrush(Colors.Transparent);
            pin_hdy.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_hdy);

            Pushpin pin_dud = new Pushpin();
            pin_dud.Location = dud;
            pin_dud.Name = "동의대";
            pin_dud.Tag = pin_dud.Name;
            pin_dud.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_dud.Background = new SolidColorBrush(Colors.Transparent);
            pin_dud.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_dud);

            Pushpin pin_bsh = new Pushpin();
            pin_bsh.Location = bsh;
            pin_bsh.Name = "부산";
            pin_bsh.Tag = pin_bsh.Name;
            pin_bsh.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_bsh.Background = new SolidColorBrush(Colors.Transparent);
            pin_bsh.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_bsh);

            Pushpin pin_hud = new Pushpin();
            pin_hud.Location = hud;
            pin_hud.Name = "해운대";
            pin_hud.Tag = pin_hud.Name;
            pin_hud.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_hud.Background = new SolidColorBrush(Colors.Transparent);
            pin_hud.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_hud);

            Pushpin pin_bsu = new Pushpin();
            pin_bsu.Location = bsu;
            pin_bsu.Name = "부산대학로";
            pin_bsu.Tag = pin_bsu.Name;
            pin_bsu.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_bsu.Background = new SolidColorBrush(Colors.Transparent);
            pin_bsu.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_bsu);

            Pushpin pin_jjd = new Pushpin();
            pin_jjd.Location = jjd;
            pin_jjd.Name = "장전동";
            pin_jjd.Tag = pin_jjd.Name;
            pin_jjd.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_jjd.Background = new SolidColorBrush(Colors.Transparent);
            pin_jjd.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_jjd);

            Pushpin pin_dgu = new Pushpin();
            pin_dgu.Location = dgu;
            pin_dgu.Name = "동의과학대학";
            pin_dgu.Tag = pin_dgu.Name;
            pin_dgu.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_dgu.Background = new SolidColorBrush(Colors.Transparent);
            pin_dgu.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_dgu);


            bloodMap.SetView(seoul, zoom);//지도 보이기

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
                GeoCoordinate dbh = new GeoCoordinate(37.6359564, 127.068234);//서울동부 하계역
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
            private void gghBt_Click(object sender, EventArgs e)
            {
                GeoCoordinate gcenter = new GeoCoordinate(37.3449811, 127.0343714);//광교산
                bloodMap.SetView(gcenter, 11);
                //Do work for your application here.
            }
            private void ichBt_Click(object sender, EventArgs e)
            {
                GeoCoordinate bcs = new GeoCoordinate(37.4844107, 126.7835564);//부천
                bloodMap.SetView(bcs, 12);
                //Do work for your application here.
            }
            private void bshBt_Click(object sender, EventArgs e)
            {
                GeoCoordinate dgu = new GeoCoordinate(35.1658384, 129.0722408);//동의과학대학
                bloodMap.SetView(dgu, 12);
                //Do work for your application here.
            }
            private void infoBt_Click(object sender, EventArgs e)
            {
                //Do work for your application here.
                MessageBox.Show("헌혈의집 위치 및 운영안내\n업데이트 문의:lispro06@gmail.com");
            }
    }
}