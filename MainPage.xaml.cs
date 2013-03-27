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
        bool loc_use = true;//위치 정보 사용 플래그

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
	        {"동의과학대학", "평일 : 10:00~19:00 \n전화번호 : 051-861-5505\n부산 부산진구 가야3동 산 72번지 동의과학대학 진리관"},
            {"강릉", "평,(토) : 09:00(10:00)~20:00 \n전화번호 : 033-647-3460\n강원 강릉시 교1동 1884-1 (교동 택지내 분수공원 앞 롯데리아 옆 위치)"},
	        {"중앙로", "평일 : 09:00~18:00 \n전화번호 : 033-252-3085\n강원 춘천시 중앙로1가 45 대한적십자사 강원도지사 1층"},
	        {"강원대", "평,(토) : 09:00(10:00)~20:00 \n전화번호 : 033-253-5551\n강원 춘천시 효자2동 강원대학교내(천지관건물1층)"},
	        {"강원", "평일 : 09:00~18:00 \n전화번호 : 033-269-1043\n강원 춘천시 퇴계동 862-3"},
	        {"상지대", "평일 : 09:00~18:00 \n전화번호 : 033-748-4012\n강원 원주시 우산동 상지대학교 창조관 3층"},
	        {"원주", "평,(토) : 09:00(10:00)~18:00 \n전화번호 : 033-745-6551\n강원 원주시 일산동 211번지 원주보건소내"},
	        {"충주", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 043-842-6262\n충북 충주시 성서동 183 (현대타운 앞 라코스테 2층)"},
	        {"천안", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 041-561-2166\n충남 천안시 동남구 신부동 462-7 문타워(문치과) 6층(터미널사거리 신세계백화점 맞은편)"},
	        {"청대앞", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 043-268-2656\n충북 청주시 상당구 우암동 237-13(청주대 앞 롯데리아 옆 버스정류장)"},
		    {"성안길", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 043-258-1649\n충북 청주시 상당구 북문로1가 175-5 청주빌딩3층(성안길 northface매장 3층)"},
	        {"충북대", "평일 : 09:30~18:30 \n전화번호 : 043-265-2655\n충북 청주시 흥덕구 개신동 (충북대학교 학생회관 2층 보건소 옆)"},
	        {"충북", "평일 : 09:30~17:30 \n전화번호 : 043-253-2654\n충북 청주시 흥덕구 휴암동318-14 (청주IC 방향 가로수길 예비군 훈련장 옆)"},
	        {"공주대학교", "평,공 : 09:00~18:00 \n전화번호 : 041-858-2166\n충남 공주시 신관동 182 공주대학교 (중앙도서관 앞)"},
	        {"충남대학교", "평일 : 09:00~18:00 \n전화번호 : 042-823-7166\n대전 유성구 궁동 220 충남대학교 (제2학생회관 3층)"},
	        {"둔산", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 042-486-2166\n대전 서구 둔산동 1039 (402호)"},
	        {"으능정이", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 042-252-2166\n대전 중구 은행동 48-2번지 2층 (은행동 이안경원 옆 건물임)"},
	        {"대전복합터미널", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 041-858-2166\n대전 동구 성남동 494-8"},
	        {"대전세종충남", "일시운영중단 \n전화번호 : 042-623-2166\n대전 대덕구 송촌동 294-6 (스파플러스 찜질방 후문 옆)"},
            {"안동", "평,(동절기) : 10:00(09:00)~19:00(18:00) \n전화번호 : 054-858-3780\n경북 안동시 남부동 242-1번지(시내 농협중구지점 바로 맞은편 건물2층)"},
	        {"포항", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 054-244-8891\n경북 포항시 북구 대흥동 594-3번지 2층(롯데시네마를 바라보고 우측편)"},
	        {"대구대", "평일 : 09:00~18:00 \n전화번호 : 053-851-3124\n경북 경산시 진량읍 대구대학교경산캠퍼스 제1 학생회관 옆"},
	        {"대구보건대학", "공사중 \n전화번호 : 053-326-9064\n대구 북구 태전동 산7번지(대구보건대학 내 문화관1층)"},
	        {"경북대", "평일 : 10:00~19:00 \n전화번호 : 053-421-6235\n대구 북구 산격동 1341-2번지 4층"},
	        {"대구경북", "평일 : 09:00~18:00 \n전화번호 : 053-605-5642\n대구 중구 달성동 147-2번지(달성네거리->북비산네거리방향200m도로변)"},
	        {"동성로", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 053-252-2285\n대구 중구 동성로2가 56-3번지 정호빌딩2층(중앙네거리->2.28공원방향 조금올라오면 도로변 귀금속건물2층)"},
	        {"228기념중앙공원", "평,토 : 10:00~19:00 \n전화번호 : 053-253-2280\n대구 중구 공평동 15(2.28기념공원 뜨라래레스토랑 맞은편)"},
	        {"대구중앙로", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 053-252-2315\n대구 중구 동성로3가 111 센트럴엠 201호"},
		    {"반월당", "평일 : 10:00~19:00 \n전화번호 : 053-254-2901\n대구 중구 남산2동 937번지(반월당네거리 대구적십자병원옆 동아쇼핑건너편)"},
	        {"울산삼산동", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 052-266-5225\n울산 남구 삼산동킴스빌딩 2층"},
	        {"성남동", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 052-243-8799\n울산 중구 성남동 219-56(미스터피자옆2층)"},
	        {"공업탑", "평,토 : 10:00~19:00 \n전화번호 : 052-260-7918\n울산 남구 신정4동 1233-7 (롯데리아 옆 1층)"},
	        {"울산대", "평일 : 09:00~18:00 \n전화번호 : 052-224-3969\n울산 남구 무거동 산 29번지(울산대학교 내 대학회관 식당 옆)"},
	        {"울산", "평일 : 09:00~18:00 \n전화번호 : 052-245-2982\n울산 중구 성안동 872-5 (성안동 옥사우나 밑으로 100m)"},
            {"김해", "평(토),(일) : 09:00(10:00)~20:00(18:00) \n전화번호 : 055-333-2612\n경남 김해시 내외중앙로 59 햄튼타워 2층"},
	        {"경남", "평일 : 09:00~18:00 \n전화번호 : 055-262-5161\n경남 창원시 용호동 4-4(창원세무서 뒷편, 롯데아파트 상가 맞은편)"},
	        {"창원", "평(토),(일) : 09:00(10:00)~20:00(18:00) \n전화번호 : 053-851-3124\n경남 창원시 용호동 정우상가 2층"},
	        {"경남대앞", "평(토) : 09:00(10:00)~18:00(17:00) \n전화번호 : 055-245-5161\n경남 창원시 마산합포구 월남동5가 4-69번지 2층(경남대 앞 삼우빌딩 2층)"},
	        {"진주", "평(토),(일) : 09:00(10:00)~20:00(18:00) \n전화번호 : 055-745-2611\n경남 진주시 대안동 17-5 2층(구 이성수 안과)"},
	        {"전남대", "평,토(일) : 10:00~19:00(18:00) \n전화번호 : 062-529-9494\n전남 광주 북구 용봉동 300번지 (전남대학교 후문 옆)"},
	        {"충장로", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 062-232-9494\n광주 동구 충장로3가 32-3 (밀리오레 맞은편)"},
	        {"금남로", "평일 : 10:00~19:00 \n전화번호 : 053-851-3124\n광주 동구 금남로 금남지하상가 H동 4호 (남도예술회관쪽 입구)"},
	        {"조선대", "평,공 : 10:00~19:00 \n전화번호 : 062-944-9494\n광주 동구 서석동 351-1번지 (조선대학교 후문 옆, 미술대학 옆)"},
	        {"광주전남", "평일 : 09:00~18:00 \n전화번호 : 062-600-0600\n광주 남구 서문로 521 (송하동 127-4, 광주인성고등학교 옆, 효천역 맞은편)"},
	        {"유달", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 061-242-4245\n전남 목포시 상락동 2가 12-4 (코롬반제과점 맞은편, 목포역전 맛의거리 근처)"},
	        {"순천", "평,(일) : 10:00~20:00(18:00) \n전화번호 : 061-753-2239\n전남 순천시 중앙동 61-23 (지하상가 입구 옆)"},
	        {"여수", "평,토(일) : 10:00~20:00(18:00) \n전화번호 : 061-663-2413\n전남 여수시 교동 304-3 (롯데리아 건물 2층)"},
	        {"정읍", "월수목 : 09:00~18:00 \n전화번호 : 가두\n전북 정읍시 시기동 267-5"},
	        {"효자", "평,토(일) : 09:00~20:00(18:00) \n전화번호 : 063-229-2116\n전북 전주시 완산구 효자동1가 435-2 전주크리닉센터 4F"},
	        {"고사동", "평,토(일) : 09:00~20:00(18:00) \n전화번호 : 063-285-2114\n전북 전주시 완산구 고사동 23-1(교보문고 사거리 세븐일레븐 2층)"},
	        {"덕진", "평일 : 09:30~18:30 \n전화번호 : 063-275-2114\n전주시 덕진구 덕진동 1가 1266-15(경기장 맞은편 덕진지하도 입구 2층)"},
	        {"전북대", "평일 : 09:00~18:00 \n전화번호 : 062-600-0600\n전북 전주시 덕진구 덕진동1가 전북대학교 (전북대 1학생회관 옆)"},
	        {"익산", "평,토(일) : 09:00~20:00(18:00) \n전화번호 : 063-856-2110\n전북 익산시 신동 762-10"},
	        {"원광대", "평일 : 09:00~18:00 \n전화번호 : 063-842-6709\n전북 익산시 신용동 원광대학교 (원광대 내 박물관,버스승강장옆)"},
	        {"군산", "평,토 : 09:30~18:30 \n전화번호 : 063-466-0609\n전북 군산시 수송동 810-3 효원월드타워 2층 (롯데마트 맞은편)"},
		    {"군산대", "평일 : 09:00~18:00 \n전화번호 : 063-463-7455\n전북 군산시 미룡동 군산대학교 학생회관 지하층 우체국옆"},
            };
        // Constructor       
        public MainPage()
        {
            InitializeComponent();
            //위치 정보 사용 여부 질의
            var result = MessageBox.Show(
             "이 앱은 현재 위치 정보를 사용합니다. " +
             "사용자 위치 정보 사용에 동의 하십니까?",
             "사용자 위치 정보",
             MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.Cancel)
            {
                //Enter code here
                loc_use = false;
            }

            // 메뉴바

                 ApplicationBar = new ApplicationBar();
                 ApplicationBarIconButton hereBt = new ApplicationBarIconButton();
                 hereBt.IconUri = new Uri("/icons/appbar.check.rest.png", UriKind.Relative);
                 hereBt.Text = "현재 위치로";
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


                 ApplicationBarMenuItem kwdBt = new ApplicationBarMenuItem("강원지역");
                 ApplicationBar.MenuItems.Add(kwdBt);

                 kwdBt.Click += new EventHandler(kwdBt_Click);


                 ApplicationBarMenuItem ccdBt = new ApplicationBarMenuItem("충청지역");
                 ApplicationBar.MenuItems.Add(ccdBt);

                 ccdBt.Click += new EventHandler(ccdBt_Click);

                 ApplicationBarMenuItem tkhBt = new ApplicationBarMenuItem("대구경북");
                 ApplicationBar.MenuItems.Add(tkhBt);

                 tkhBt.Click += new EventHandler(tkhBt_Click);


                 ApplicationBarMenuItem ushBt = new ApplicationBarMenuItem("울산지역");
                 ApplicationBar.MenuItems.Add(ushBt);

                 ushBt.Click += new EventHandler(ushBt_Click);

                 ApplicationBarMenuItem knuBt = new ApplicationBarMenuItem("경남지역");
                 ApplicationBar.MenuItems.Add(knuBt);

                 knuBt.Click += new EventHandler(knuBt_Click);


                 ApplicationBarMenuItem gjnBt = new ApplicationBarMenuItem("광주전남");
                 ApplicationBar.MenuItems.Add(gjnBt);

                 gjnBt.Click += new EventHandler(gjnBt_Click);

                 ApplicationBarMenuItem gjsBt = new ApplicationBarMenuItem("전북지역");
                 ApplicationBar.MenuItems.Add(gjsBt);

                 gjsBt.Click += new EventHandler(gjsBt_Click);


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

            // 강원도
                GeoCoordinate gyo = new GeoCoordinate(37.7641382, 128.8760538);//강릉
                GeoCoordinate jar = new GeoCoordinate(37.8831955, 127.7292735);//중앙로
                GeoCoordinate kwu = new GeoCoordinate(37.8706622, 127.7443964);//강원대
                GeoCoordinate kwd = new GeoCoordinate(37.8461548, 127.7386211);//강원
                GeoCoordinate sgu = new GeoCoordinate(37.36894, 127.9308374);//상지대센터
                GeoCoordinate wjc = new GeoCoordinate(37.3515742, 127.9468211);//원주

            // 충청도
                GeoCoordinate cjc = new GeoCoordinate(36.9713546, 127.9317317);//충주
                GeoCoordinate cac = new GeoCoordinate(36.8186642, 127.1578158);//천안
                GeoCoordinate cju = new GeoCoordinate(36.6508175, 127.4885893);//청대앞
                GeoCoordinate sag = new GeoCoordinate(36.6345473, 127.4890011);//성안길
                GeoCoordinate cbu = new GeoCoordinate(36.6276033, 127.4587962);//충북대
                GeoCoordinate had = new GeoCoordinate(36.6227781, 127.408816);//충북
                GeoCoordinate kju = new GeoCoordinate(36.469352, 127.1398843);//공주대학교
                GeoCoordinate cnu = new GeoCoordinate(36.3659634, 127.3458188);//충남대학교
                GeoCoordinate dsd = new GeoCoordinate(36.3513717, 127.3775162);//둔산
                GeoCoordinate unj = new GeoCoordinate(36.3288993, 127.427665);//으느정이
                GeoCoordinate dbt = new GeoCoordinate(36.3490706, 127.4375941);//대전복합터미널
                GeoCoordinate dsc = new GeoCoordinate(36.3539339, 127.4420451);//대전.세종.충남



            //deagu
                GeoCoordinate adc = new GeoCoordinate(36.5640022, 128.7305919);//안동
                GeoCoordinate phc = new GeoCoordinate(36.0363686, 129.3624915);//포항
                GeoCoordinate tgu = new GeoCoordinate(35.901332, 129.3624915);//대구대
                GeoCoordinate tbu = new GeoCoordinate(35.928578, 128.543535);//대구보건대학
                GeoCoordinate gbu = new GeoCoordinate(35.8928153, 128.6091775);//경북대
                GeoCoordinate tkh = new GeoCoordinate(35.8786051, 128.5790704);//대구경북
                GeoCoordinate dsr = new GeoCoordinate(35.8705104, 128.5960519);//동성로
                GeoCoordinate fmp = new GeoCoordinate(35.8695897, 128.5982523);//2.28기념중앙공원
                GeoCoordinate tja = new GeoCoordinate(35.8673488, 128.5950481);//중앙로
                GeoCoordinate pwd = new GeoCoordinate(35.8654904, 128.5916133);//반월당

            //ulsan
                GeoCoordinate ssd = new GeoCoordinate(35.5396311, 129.3371712);//삼산동
                GeoCoordinate snd = new GeoCoordinate(35.5537313, 129.3200672);//성남동
                GeoCoordinate kut = new GeoCoordinate(35.5323575, 129.3089138);//공업탑
                GeoCoordinate usu = new GeoCoordinate(35.544583, 129.257225);//울산대
                GeoCoordinate ush = new GeoCoordinate(35.5731502, 129.3086092);//울산

            //경남
                GeoCoordinate khc = new GeoCoordinate(35.235477, 128.8664925);//김해
                GeoCoordinate knh = new GeoCoordinate(35.2334781, 128.6860005);//경남
                GeoCoordinate cwc = new GeoCoordinate(35.228355, 128.6799009);//창원
                GeoCoordinate knu = new GeoCoordinate(35.1807293, 128.5593173);//경남대앞
                GeoCoordinate jjc = new GeoCoordinate(35.1950882, 128.0824996);//진주

            //광주전남
                GeoCoordinate jnu = new GeoCoordinate(35.175168, 126.912025);//전남대
                GeoCoordinate cjr = new GeoCoordinate(35.1489697, 126.9143073);//충장로
                GeoCoordinate gnr = new GeoCoordinate(35.1477717, 126.9198781);//금남로
                GeoCoordinate csu = new GeoCoordinate(35.1440699, 126.9303996);//조선대
                GeoCoordinate gjh = new GeoCoordinate(35.1040514, 126.8804684);//광주전남
                GeoCoordinate mpc = new GeoCoordinate(34.7897483, 126.3848954);//유달
                GeoCoordinate scc = new GeoCoordinate(34.9561493, 127.4844415);//순천
                GeoCoordinate ysc = new GeoCoordinate(34.7408346, 127.7338797);//여수
            //전북
                GeoCoordinate juc = new GeoCoordinate(35.5625321, 126.8532685);//정읍
                GeoCoordinate hjd = new GeoCoordinate(35.8068913, 127.116345);//효자
                GeoCoordinate gsd = new GeoCoordinate(35.8202331, 127.1452992);//고사동
                GeoCoordinate djd = new GeoCoordinate(35.8421622, 127.1265475);//덕진
                GeoCoordinate jbu = new GeoCoordinate(35.8458321, 127.1281125);//전북대
                GeoCoordinate isc = new GeoCoordinate(35.9638879, 126.9568928);//익산
                GeoCoordinate wku = new GeoCoordinate(35.9663013, 126.9567125);//원광대
                GeoCoordinate gsc = new GeoCoordinate(35.9648481, 126.7150938);//군산
                GeoCoordinate gsu = new GeoCoordinate(35.9460102, 126.6820989);//군산대
            int zoom = 12;


            // 아이콘을 rectanglar로 지정
            Uri imgUri = new Uri("icons/blood-donate-icon.png", UriKind.RelativeOrAbsolute);
            BitmapImage imgSourceR = new BitmapImage(imgUri);
            ImageBrush imgBrush = new ImageBrush() { ImageSource = imgSourceR };
            // 아이콘을 rectanglar로 지정
            Uri imgUri2 = new Uri("icons/getImage.png", UriKind.RelativeOrAbsolute);
            BitmapImage imgSourceR2 = new BitmapImage(imgUri2);
            ImageBrush imgBrush2 = new ImageBrush() { ImageSource = imgSourceR2 };


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
            // 부산
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

            // 강원도
            // Create a pushpin to put at the center of the view
            Pushpin pin_gyo = new Pushpin();
            pin_gyo.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            pin_gyo.Background = new SolidColorBrush(Colors.Transparent);
            pin_gyo.Location = gyo;
            pin_gyo.Name = "강릉";
            pin_gyo.Tag = pin_gyo.Name;
            pin_gyo.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            bloodMap.Children.Add(pin_gyo);

            
            Pushpin pin_jar = new Pushpin();
            pin_jar.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            pin_jar.Background = new SolidColorBrush(Colors.Transparent);
            pin_jar.Location = jar;
            pin_jar.Name = "중앙로";
            pin_jar.Tag = pin_jar.Name;
            pin_jar.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            bloodMap.Children.Add(pin_jar);

            
            Pushpin pin_kwu = new Pushpin();
            pin_kwu.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            pin_kwu.Background = new SolidColorBrush(Colors.Transparent);
            pin_kwu.Location = kwu;
            pin_kwu.Name = "강원대";
            pin_kwu.Tag = pin_kwu.Name;
            pin_kwu.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            bloodMap.Children.Add(pin_kwu);
            

            Pushpin pin_sgu = new Pushpin();
            pin_sgu.Location = sgu;
            pin_sgu.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            pin_sgu.Name = "상지대";
            pin_sgu.Tag = pin_sgu.Name;
            pin_sgu.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_sgu.Background = new SolidColorBrush(Colors.Transparent);
            bloodMap.Children.Add(pin_sgu);

            Pushpin pin_wjc = new Pushpin();
            pin_wjc.Location = wjc;
            pin_wjc.Name = "원주";
            pin_wjc.Tag = pin_wjc.Name;
            pin_wjc.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_wjc.Background = new SolidColorBrush(Colors.Transparent);
            pin_wjc.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_wjc);

            // 충청도
            Pushpin pin_cjc = new Pushpin();
            pin_cjc.Location = cjc;
            pin_cjc.Name = "충주";
            pin_cjc.Tag = pin_cjc.Name;
            pin_cjc.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_cjc.Background = new SolidColorBrush(Colors.Transparent);
            pin_cjc.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_cjc);

        
            Pushpin pin_cac = new Pushpin();
            pin_cac.Location = cac;
            pin_cac.Name = "천안";
            pin_cac.Tag = pin_cac.Name;
            pin_cac.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_cac.Background = new SolidColorBrush(Colors.Transparent);
            pin_cac.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_cac);


            Pushpin pin_cju = new Pushpin();
            pin_cju.Location = cju;
            pin_cju.Name = "청대앞";
            pin_cju.Tag = pin_cju.Name;
            pin_cju.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_cju.Background = new SolidColorBrush(Colors.Transparent);
            pin_cju.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_cju);

            Pushpin pin_sag = new Pushpin();
            pin_sag.Location = sag;
            pin_sag.Name = "성안길";
            pin_sag.Tag = pin_sag.Name;
            pin_sag.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_sag.Background = new SolidColorBrush(Colors.Transparent);
            pin_sag.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_sag);

            Pushpin pin_cbu = new Pushpin();
            pin_cbu.Location = cbu;
            pin_cbu.Name = "충북대";
            pin_cbu.Tag = pin_cbu.Name;
            pin_cbu.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_cbu.Background = new SolidColorBrush(Colors.Transparent);
            pin_cbu.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_cbu);

            Pushpin pin_had = new Pushpin();
            pin_had.Location = had;
            pin_had.Name = "충북";
            pin_had.Tag = pin_had.Name;
            pin_had.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_had.Background = new SolidColorBrush(Colors.Transparent);
            pin_had.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_had);

            // Create a pushpin to put at the center of the view
            Pushpin pin_kju = new Pushpin();
            pin_kju.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            pin_kju.Background = new SolidColorBrush(Colors.Transparent);
            pin_kju.Location = kju;
            pin_kju.Name = "공주대학교";
            pin_kju.Tag = pin_kju.Name;
            pin_kju.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            bloodMap.Children.Add(pin_kju);

            // Create a pushpin to put at the center of the view
            Pushpin pin_cnu = new Pushpin();
            pin_cnu.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            pin_cnu.Background = new SolidColorBrush(Colors.Transparent);
            pin_cnu.Location = cnu;
            pin_cnu.Name = "충남대학교";
            pin_cnu.Tag = pin_cnu.Name;
            pin_cnu.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            bloodMap.Children.Add(pin_cnu);

            // Create a pushpin to put at the center of the view
            Pushpin pin_dsd = new Pushpin();
            pin_dsd.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            pin_dsd.Background = new SolidColorBrush(Colors.Transparent);
            pin_dsd.Location = dsd;
            pin_dsd.Name = "둔산";
            pin_dsd.Tag = pin_dsd.Name;
            pin_dsd.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            bloodMap.Children.Add(pin_dsd);
            // Create a pushpin to put at the center of the view

            Pushpin pin_unj = new Pushpin();
            pin_unj.Location = unj;
            pin_unj.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            pin_unj.Name = "으능정이";
            pin_unj.Tag = pin_unj.Name;
            pin_unj.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_unj.Background = new SolidColorBrush(Colors.Transparent);
            bloodMap.Children.Add(pin_unj);

            Pushpin pin_dbt = new Pushpin();
            pin_dbt.Location = dbt;
            pin_dbt.Name = "대전복합터미널";
            pin_dbt.Tag = pin_dbt.Name;
            pin_dbt.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_dbt.Background = new SolidColorBrush(Colors.Transparent);
            pin_dbt.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_dbt);

            Pushpin pin_dsc = new Pushpin();
            pin_dsc.Location = dsc;
            pin_dsc.Name = "대전세종충남";
            pin_dsc.Tag = pin_dsc.Name;
            pin_dsc.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_dsc.Background = new SolidColorBrush(Colors.Transparent);
            pin_dsc.Content = new Rectangle()
            {
                Fill = imgBrush2,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_dsc);

            // Create a pushpin to put at the center of the view
            Pushpin pin_adc = new Pushpin();
            pin_adc.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            pin_adc.Background = new SolidColorBrush(Colors.Transparent);
            pin_adc.Location = adc;
            pin_adc.Name = "안동";
            pin_adc.Tag = pin_adc.Name;
            pin_adc.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            bloodMap.Children.Add(pin_adc);

            // Create a pushpin to put at the center of the view
            Pushpin pin_phc = new Pushpin();
            pin_phc.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            pin_phc.Background = new SolidColorBrush(Colors.Transparent);
            pin_phc.Location = phc;
            pin_phc.Name = "포항";
            pin_phc.Tag = pin_phc.Name;
            pin_phc.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            bloodMap.Children.Add(pin_phc);

            // Create a pushpin to put at the center of the view
            Pushpin pin_tgu = new Pushpin();
            pin_tgu.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            pin_tgu.Background = new SolidColorBrush(Colors.Transparent);
            pin_tgu.Location = tgu;
            pin_tgu.Name = "대구대";
            pin_tgu.Tag = pin_tgu.Name;
            pin_tgu.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            bloodMap.Children.Add(pin_tgu);
            // Create a pushpin to put at the center of the view

            Pushpin pin_tbu = new Pushpin();
            pin_tbu.Location = tbu;
            pin_tbu.Content = new Rectangle()
            {
                Fill = imgBrush2,
                Height = 64,
                Width = 64
            };
            pin_tbu.Name = "대구보건대학";
            pin_tbu.Tag = pin_tbu.Name;
            pin_tbu.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_tbu.Background = new SolidColorBrush(Colors.Transparent);
            bloodMap.Children.Add(pin_tbu);

            Pushpin pin_gbu = new Pushpin();
            pin_gbu.Location = gbu;
            pin_gbu.Name = "경북대";
            pin_gbu.Tag = pin_gbu.Name;
            pin_gbu.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_gbu.Background = new SolidColorBrush(Colors.Transparent);
            pin_gbu.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_gbu);

            Pushpin pin_tkh = new Pushpin();
            pin_tkh.Location = tkh;
            pin_tkh.Name = "대구경북";
            pin_tkh.Tag = pin_tkh.Name;
            pin_tkh.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_tkh.Background = new SolidColorBrush(Colors.Transparent);
            pin_tkh.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_tkh);




            Pushpin pin_dsr = new Pushpin();
            pin_dsr.Location = dsr;
            pin_dsr.Name = "동성로";
            pin_dsr.Tag = pin_dsr.Name;
            pin_dsr.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_dsr.Background = new SolidColorBrush(Colors.Transparent);
            pin_dsr.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_dsr);


            Pushpin pin_fmp = new Pushpin();
            pin_fmp.Location = fmp;
            pin_fmp.Name = "228기념중앙공원";
            pin_fmp.Tag = pin_fmp.Name;
            pin_fmp.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_fmp.Background = new SolidColorBrush(Colors.Transparent);
            pin_fmp.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_fmp);

            Pushpin pin_tja = new Pushpin();
            pin_tja.Location = tja;
            pin_tja.Name = "대구중앙로";
            pin_tja.Tag = pin_tja.Name;
            pin_tja.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_tja.Background = new SolidColorBrush(Colors.Transparent);
            pin_tja.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_tja);

            Pushpin pin_pwd = new Pushpin();
            pin_pwd.Location = pwd;
            pin_pwd.Name = "반월당";
            pin_pwd.Tag = pin_pwd.Name;
            pin_pwd.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_pwd.Background = new SolidColorBrush(Colors.Transparent);
            pin_pwd.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_pwd);
            Pushpin pin_ssd = new Pushpin();
            pin_ssd.Location = ssd;
            pin_ssd.Name = "울산삼산동";
            pin_ssd.Tag = pin_ssd.Name;
            pin_ssd.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_ssd.Background = new SolidColorBrush(Colors.Transparent);
            pin_ssd.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_ssd);

            Pushpin pin_snd = new Pushpin();
            pin_snd.Location = snd;
            pin_snd.Name = "성남동";
            pin_snd.Tag = pin_snd.Name;
            pin_snd.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_snd.Background = new SolidColorBrush(Colors.Transparent);
            pin_snd.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_snd);

            Pushpin pin_kut = new Pushpin();
            pin_kut.Location = kut;
            pin_kut.Name = "공업탑";
            pin_kut.Tag = pin_kut.Name;
            pin_kut.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_kut.Background = new SolidColorBrush(Colors.Transparent);
            pin_kut.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_kut);


            Pushpin pin_usu = new Pushpin();
            pin_usu.Location = usu;
            pin_usu.Name = "울산대";
            pin_usu.Tag = pin_usu.Name;
            pin_usu.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_usu.Background = new SolidColorBrush(Colors.Transparent);
            pin_usu.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_usu);

            Pushpin pin_ush = new Pushpin();
            pin_ush.Location = ush;
            pin_ush.Name = "울산";
            pin_ush.Tag = pin_grc.Name;
            pin_ush.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_ush.Background = new SolidColorBrush(Colors.Transparent);
            pin_ush.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_ush);

            //경남
            // Create a pushpin to put at the center of the view
            Pushpin pin_khc = new Pushpin();
            pin_khc.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            pin_khc.Background = new SolidColorBrush(Colors.Transparent);
            pin_khc.Location = khc;
            pin_khc.Name = "김해";
            pin_khc.Tag = pin_khc.Name;
            pin_khc.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            bloodMap.Children.Add(pin_khc);

            // Create a pushpin to put at the center of the view
            Pushpin pin_cwc = new Pushpin();
            pin_cwc.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            pin_cwc.Background = new SolidColorBrush(Colors.Transparent);
            pin_cwc.Location = cwc;
            pin_cwc.Name = "창원";
            pin_cwc.Tag = pin_cwc.Name;
            pin_cwc.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            bloodMap.Children.Add(pin_cwc);

            // Create a pushpin to put at the center of the view
            Pushpin pin_knh = new Pushpin();
            pin_knh.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            pin_knh.Background = new SolidColorBrush(Colors.Transparent);
            pin_knh.Location = knh;
            pin_knh.Name = "경남";
            pin_knh.Tag = pin_knh.Name;
            pin_knh.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            bloodMap.Children.Add(pin_knh);
            // Create a pushpin to put at the center of the view

            Pushpin pin_knu = new Pushpin();
            pin_knu.Location = knu;
            pin_knu.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            pin_knu.Name = "경남대앞";
            pin_knu.Tag = pin_knu.Name;
            pin_knu.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_knu.Background = new SolidColorBrush(Colors.Transparent);
            bloodMap.Children.Add(pin_knu);

            Pushpin pin_jjc = new Pushpin();
            pin_jjc.Location = jjc;
            pin_jjc.Name = "진주";
            pin_jjc.Tag = pin_jjc.Name;
            pin_jjc.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_jjc.Background = new SolidColorBrush(Colors.Transparent);
            pin_jjc.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_jjc);


            // Create a pushpin to put at the center of the view
            Pushpin pin_jnu = new Pushpin();
            pin_jnu.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            pin_jnu.Background = new SolidColorBrush(Colors.Transparent);
            pin_jnu.Location = jnu;
            pin_jnu.Name = "전남대";
            pin_jnu.Tag = pin_jnu.Name;
            pin_jnu.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            bloodMap.Children.Add(pin_jnu);

            // Create a pushpin to put at the center of the view
            Pushpin pin_cjr = new Pushpin();
            pin_cjr.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            pin_cjr.Background = new SolidColorBrush(Colors.Transparent);
            pin_cjr.Location = cjr;
            pin_cjr.Name = "충장로";
            pin_cjr.Tag = pin_cjr.Name;
            pin_cjr.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            bloodMap.Children.Add(pin_cjr);

            // Create a pushpin to put at the center of the view
            Pushpin pin_gnr = new Pushpin();
            pin_gnr.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            pin_gnr.Background = new SolidColorBrush(Colors.Transparent);
            pin_gnr.Location = gnr;
            pin_gnr.Name = "금남로";
            pin_gnr.Tag = pin_gnr.Name;
            pin_gnr.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            bloodMap.Children.Add(pin_gnr);
            // Create a pushpin to put at the center of the view

            Pushpin pin_csu = new Pushpin();
            pin_csu.Location = csu;
            pin_csu.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            pin_csu.Name = "조선대";
            pin_csu.Tag = pin_csu.Name;
            pin_csu.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_csu.Background = new SolidColorBrush(Colors.Transparent);
            bloodMap.Children.Add(pin_csu);

            Pushpin pin_gjh = new Pushpin();
            pin_gjh.Location = gjh;
            pin_gjh.Name = "광주전남";
            pin_gjh.Tag = pin_gjh.Name;
            pin_gjh.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_gjh.Background = new SolidColorBrush(Colors.Transparent);
            pin_gjh.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_gjh);

            Pushpin pin_mpc = new Pushpin();
            pin_mpc.Location = mpc;
            pin_mpc.Name = "유달";
            pin_mpc.Tag = pin_mpc.Name;
            pin_mpc.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_mpc.Background = new SolidColorBrush(Colors.Transparent);
            pin_mpc.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_mpc);


            Pushpin pin_scc = new Pushpin();
            pin_scc.Location = scc;
            pin_scc.Name = "순천";
            pin_scc.Tag = pin_scc.Name;
            pin_scc.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_scc.Background = new SolidColorBrush(Colors.Transparent);
            pin_scc.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_scc);


            Pushpin pin_ysc = new Pushpin();
            pin_ysc.Location = ysc;
            pin_ysc.Name = "여수";
            pin_ysc.Tag = pin_ysc.Name;
            pin_ysc.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_ysc.Background = new SolidColorBrush(Colors.Transparent);
            pin_ysc.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_ysc);


            // Create a pushpin to put at the center of the view
            Pushpin pin_juc = new Pushpin();
            pin_juc.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            pin_juc.Background = new SolidColorBrush(Colors.Transparent);
            pin_juc.Location = juc;
            pin_juc.Name = "정읍";
            pin_juc.Tag = pin_juc.Name;
            pin_juc.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            bloodMap.Children.Add(pin_juc);

            // Create a pushpin to put at the center of the view
            Pushpin pin_hjd = new Pushpin();
            pin_hjd.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            pin_hjd.Background = new SolidColorBrush(Colors.Transparent);
            pin_hjd.Location = hjd;
            pin_hjd.Name = "효자";
            pin_hjd.Tag = pin_hjd.Name;
            pin_hjd.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            bloodMap.Children.Add(pin_hjd);

            // Create a pushpin to put at the center of the view
            Pushpin pin_gsd = new Pushpin();
            pin_gsd.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            pin_gsd.Background = new SolidColorBrush(Colors.Transparent);
            pin_gsd.Location = gsd;
            pin_gsd.Name = "고사동";
            pin_gsd.Tag = pin_gsd.Name;
            pin_gsd.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            bloodMap.Children.Add(pin_gsd);
            // Create a pushpin to put at the center of the view

            Pushpin pin_djd = new Pushpin();
            pin_djd.Location = djd;
            pin_djd.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            pin_djd.Name = "덕진";
            pin_djd.Tag = pin_djd.Name;
            pin_djd.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_djd.Background = new SolidColorBrush(Colors.Transparent);
            bloodMap.Children.Add(pin_djd);

            Pushpin pin_jbu = new Pushpin();
            pin_jbu.Location = jbu;
            pin_jbu.Name = "전북대";
            pin_jbu.Tag = pin_jbu.Name;
            pin_jbu.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_jbu.Background = new SolidColorBrush(Colors.Transparent);
            pin_jbu.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_jbu);

            Pushpin pin_isc = new Pushpin();
            pin_isc.Location = isc;
            pin_isc.Name = "익산";
            pin_isc.Tag = pin_isc.Name;
            pin_isc.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_isc.Background = new SolidColorBrush(Colors.Transparent);
            pin_isc.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_isc);


            Pushpin pin_wku = new Pushpin();
            pin_wku.Location = wku;
            pin_wku.Name = "원광대";
            pin_wku.Tag = pin_wku.Name;
            pin_wku.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_wku.Background = new SolidColorBrush(Colors.Transparent);
            pin_wku.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_wku);


            Pushpin pin_gsc = new Pushpin();
            pin_gsc.Location = gsc;
            pin_gsc.Name = "군산";
            pin_gsc.Tag = pin_gsc.Name;
            pin_gsc.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_gsc.Background = new SolidColorBrush(Colors.Transparent);
            pin_gsc.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_gsc);

            Pushpin pin_gsu = new Pushpin();
            pin_gsu.Location = gsu;
            pin_gsu.Name = "군산대";
            pin_gsu.Tag = pin_gsu.Name;
            pin_gsu.MouseLeftButtonUp += new MouseButtonEventHandler(pin1_MouseLeftButtonUp);
            pin_gsu.Background = new SolidColorBrush(Colors.Transparent);
            pin_gsu.Content = new Rectangle()
            {
                Fill = imgBrush,
                Height = 64,
                Width = 64
            };
            bloodMap.Children.Add(pin_gsu);

            bloodMap.SetView(seoul, zoom);//지도 보이기

            if (loc_use)
            {
                watcher.Start();
            }
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
                if (loc_use)
                {
                    watcher.Start();
                }
                else
                {
                    MessageBox.Show("위치 정보 사용 안함 선택");
                }
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
            private void kwdBt_Click(object sender, EventArgs e)
            {
                GeoCoordinate kwd = new GeoCoordinate(37.728343, 128.465487);//계방산
                bloodMap.SetView(kwd, 9);
                //Do work for your application here.
            }
            private void ccdBt_Click(object sender, EventArgs e)
            {
                GeoCoordinate had = new GeoCoordinate(36.6227781, 127.408816);//충북
                bloodMap.SetView(had, 9);
                //Do work for your application here.
            }
            private void tkhBt_Click(object sender, EventArgs e)
            {
                GeoCoordinate mbm = new GeoCoordinate(36.232376, 128.86895);//대구경북
                bloodMap.SetView(mbm, 9);
                //Do work for your application here.
            }
            private void ushBt_Click(object sender, EventArgs e)
            {
                GeoCoordinate ush = new GeoCoordinate(35.5731502, 129.3086092);//울산
                bloodMap.SetView(ush, 11);
                //Do work for your application here.
            }

            private void knuBt_Click(object sender, EventArgs e)
            {
                GeoCoordinate knu = new GeoCoordinate(35.1807293, 128.5593173);//경남대앞
                bloodMap.SetView(knu, 10);
                //Do work for your application here.
            }
            private void gjnBt_Click(object sender, EventArgs e)
            {
                GeoCoordinate gjh = new GeoCoordinate(35.0645238, 126.9864771);//화순군청
                bloodMap.SetView(gjh, 9);
                //Do work for your application here.
            }
            private void gjsBt_Click(object sender, EventArgs e)
            {
                GeoCoordinate gjs = new GeoCoordinate(35.848404, 126.895639);//백산저수지
                bloodMap.SetView(gjs, 10);
                //Do work for your application here.
            }
            private void infoBt_Click(object sender, EventArgs e)
            {
                //Do work for your application here.
                MessageBox.Show("헌혈의집 위치 및 운영안내\n업데이트 문의:lispro06@gmail.com");
            }
    }
}