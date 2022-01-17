using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Threading;
using System.Linq;
using System;

#pragma warning disable IDE0051 // Remove unused private members
#pragma warning disable IDE0044 // Add readonly modifier

public class Scanner : MonoBehaviour
{
	[SerializeField]	private	Transform	_cube;
	[SerializeField]	private	Transform	_sphere;
	[SerializeField]	private	Camera		_camera;
	[Range(10.0f, 80.0f)]
	[SerializeField]	private	float		_altitudeAngleDegrees	= 45.0f;
						private	float		_azimuthAngleRadians;

	private static readonly Func<Vector3Int, Vector3Int>[] transformations =
		new Func<Vector3Int, Vector3Int>[]
		{
			v => new Vector3Int(v.x, v.y, v.z),
			v => new Vector3Int(v.x, v.z, -v.y),
			v => new Vector3Int(v.x, -v.y, -v.z),
			v => new Vector3Int(v.x, -v.z, v.y),
			v => new Vector3Int(v.y, v.z, v.x),
			v => new Vector3Int(v.y, v.x, -v.z),
			v => new Vector3Int(v.y, -v.x, v.z),
			v => new Vector3Int(v.y, -v.z, -v.x),
			v => new Vector3Int(v.z, v.x, v.y),
			v => new Vector3Int(v.z, v.y, -v.x),
			v => new Vector3Int(v.z, -v.x, -v.y),
			v => new Vector3Int(v.z, -v.y, v.x),
			v => new Vector3Int(-v.x, v.z, v.y),
			v => new Vector3Int(-v.x, v.y, -v.z),
			v => new Vector3Int(-v.x, -v.y, v.z),
			v => new Vector3Int(-v.x, -v.z, -v.y),
			v => new Vector3Int(-v.y, v.x, v.z),
			v => new Vector3Int(-v.y, v.z, -v.x),
			v => new Vector3Int(-v.y, -v.x, -v.z),
			v => new Vector3Int(-v.y, -v.z, v.x),
			v => new Vector3Int(-v.z, v.y, v.x),
			v => new Vector3Int(-v.z, v.x, -v.y),
			v => new Vector3Int(-v.z, -v.x, v.y),
			v => new Vector3Int(-v.z, -v.y, -v.x),
		};

	private static readonly List<List<Vector3Int>> _scanners
		= new List<List<Vector3Int>>
		{
			new List<Vector3Int>
			{
				new Vector3Int(-775,-554,-532),
				new Vector3Int(43,-16,96),
				new Vector3Int(-784,-551,460),
				new Vector3Int(718,-337,582),
				new Vector3Int(-892,-499,-569),
				new Vector3Int(463,-600,-530),
				new Vector3Int(-815,-637,-599),
				new Vector3Int(776,-278,495),
				new Vector3Int(858,-268,526),
				new Vector3Int(293,497,-799),
				new Vector3Int(531,-580,-430),
				new Vector3Int(286,704,496),
				new Vector3Int(-718,-447,453),
				new Vector3Int(314,831,609),
				new Vector3Int(280,497,-675),
				new Vector3Int(-125,122,195),
				new Vector3Int(-917,755,-729),
				new Vector3Int(-728,-466,600),
				new Vector3Int(-910,798,-490),
				new Vector3Int(277,733,552),
				new Vector3Int(-428,756,796),
				new Vector3Int(-491,713,739),
				new Vector3Int(-425,674,850),
				new Vector3Int(469,-473,-489),
				new Vector3Int(-923,625,-580),
				new Vector3Int(374,459,-626),
			},
			new List<Vector3Int>
			{
				new Vector3Int(337,-770,-686),
				new Vector3Int(715,590,522),
				new Vector3Int(714,-784,879),
				new Vector3Int(286,-914,-728),
				new Vector3Int(371,565,-546),
				new Vector3Int(314,-890,-714),
				new Vector3Int(336,518,-686),
				new Vector3Int(-556,-575,592),
				new Vector3Int(-541,750,-562),
				new Vector3Int(-605,-753,-639),
				new Vector3Int(-708,776,377),
				new Vector3Int(736,426,566),
				new Vector3Int(778,607,538),
				new Vector3Int(-600,-640,772),
				new Vector3Int(-125,-54,56),
				new Vector3Int(35,-9,-45),
				new Vector3Int(-563,609,-523),
				new Vector3Int(-700,-738,-597),
				new Vector3Int(-738,-900,-636),
				new Vector3Int(-854,786,421),
				new Vector3Int(-639,-586,745),
				new Vector3Int(810,-713,806),
				new Vector3Int(294,597,-553),
				new Vector3Int(731,-800,785),
				new Vector3Int(-781,768,547),
				new Vector3Int(-590,720,-395),
			},
			new List<Vector3Int>
			{
				new Vector3Int(-441,556,-929),
				new Vector3Int(-327,708,-896),
				new Vector3Int(-479,696,684),
				new Vector3Int(-620,-835,781),
				new Vector3Int(543,548,-776),
				new Vector3Int(780,-569,-623),
				new Vector3Int(72,-35,-28),
				new Vector3Int(739,-671,589),
				new Vector3Int(-722,-829,846),
				new Vector3Int(751,-616,690),
				new Vector3Int(-726,-518,-431),
				new Vector3Int(614,357,-777),
				new Vector3Int(820,-560,-520),
				new Vector3Int(-395,593,590),
				new Vector3Int(409,467,503),
				new Vector3Int(-480,689,-930),
				new Vector3Int(-638,-678,796),
				new Vector3Int(-659,-697,-393),
				new Vector3Int(-670,-521,-456),
				new Vector3Int(-381,674,686),
				new Vector3Int(505,478,-820),
				new Vector3Int(724,-562,541),
				new Vector3Int(512,487,569),
				new Vector3Int(799,-727,-520),
				new Vector3Int(383,626,564),
			},
			new List<Vector3Int>
			{
				new Vector3Int(-471,-463,618),
				new Vector3Int(-528,536,550),
				new Vector3Int(580,589,470),
				new Vector3Int(611,632,475),
				new Vector3Int(-631,-616,-517),
				new Vector3Int(387,908,-363),
				new Vector3Int(-420,-552,711),
				new Vector3Int(-596,872,-676),
				new Vector3Int(558,-453,-661),
				new Vector3Int(-57,3,-53),
				new Vector3Int(343,898,-509),
				new Vector3Int(432,-607,530),
				new Vector3Int(-532,492,595),
				new Vector3Int(-673,-631,-612),
				new Vector3Int(-429,842,-627),
				new Vector3Int(513,890,-436),
				new Vector3Int(711,-464,-578),
				new Vector3Int(-458,-587,707),
				new Vector3Int(515,-559,625),
				new Vector3Int(334,-581,608),
				new Vector3Int(-670,-778,-479),
				new Vector3Int(-539,593,691),
				new Vector3Int(738,-518,-617),
				new Vector3Int(-557,731,-654),
				new Vector3Int(649,644,484),
			},
			new List<Vector3Int>
			{
				new Vector3Int(773,-717,-634),
				new Vector3Int(808,667,616),
				new Vector3Int(-324,505,684),
				new Vector3Int(129,-130,-61),
				new Vector3Int(-775,808,-500),
				new Vector3Int(-352,-815,452),
				new Vector3Int(-799,666,-540),
				new Vector3Int(66,54,34),
				new Vector3Int(535,-677,376),
				new Vector3Int(954,517,-467),
				new Vector3Int(801,-678,-463),
				new Vector3Int(749,572,-432),
				new Vector3Int(-419,-830,362),
				new Vector3Int(699,682,568),
				new Vector3Int(-816,781,-501),
				new Vector3Int(754,694,467),
				new Vector3Int(541,-779,311),
				new Vector3Int(-622,-542,-779),
				new Vector3Int(-755,-542,-767),
				new Vector3Int(-373,626,725),
				new Vector3Int(-404,568,818),
				new Vector3Int(777,-727,-350),
				new Vector3Int(-663,-338,-764),
				new Vector3Int(-274,-843,495),
				new Vector3Int(692,-695,361),
				new Vector3Int(791,545,-440),
			},
			new List<Vector3Int>
			{
				new Vector3Int(-760,593,537),
				new Vector3Int(-686,583,544),
				new Vector3Int(427,-483,728),
				new Vector3Int(-401,-648,-691),
				new Vector3Int(-570,-596,-652),
				new Vector3Int(531,634,-630),
				new Vector3Int(-519,424,-468),
				new Vector3Int(-729,419,-444),
				new Vector3Int(-738,-441,557),
				new Vector3Int(527,-382,-315),
				new Vector3Int(-788,622,509),
				new Vector3Int(693,567,425),
				new Vector3Int(-812,-366,558),
				new Vector3Int(460,-418,-286),
				new Vector3Int(393,-523,622),
				new Vector3Int(729,514,586),
				new Vector3Int(600,-440,-304),
				new Vector3Int(-741,-585,560),
				new Vector3Int(511,577,-450),
				new Vector3Int(501,-567,630),
				new Vector3Int(-22,169,14),
				new Vector3Int(-547,479,-473),
				new Vector3Int(466,587,-645),
				new Vector3Int(-516,-651,-761),
				new Vector3Int(766,630,561),
			},
			new List<Vector3Int>
			{
				new Vector3Int(-326,322,730),
				new Vector3Int(-445,-390,376),
				new Vector3Int(574,489,585),
				new Vector3Int(-735,-527,-435),
				new Vector3Int(-496,330,803),
				new Vector3Int(91,47,-14),
				new Vector3Int(-542,-548,387),
				new Vector3Int(648,-569,724),
				new Vector3Int(603,-590,-612),
				new Vector3Int(-736,-587,-407),
				new Vector3Int(-487,491,-756),
				new Vector3Int(828,-581,704),
				new Vector3Int(-527,-545,430),
				new Vector3Int(-471,508,-693),
				new Vector3Int(-768,-536,-342),
				new Vector3Int(596,-524,-524),
				new Vector3Int(-515,672,-714),
				new Vector3Int(858,-559,735),
				new Vector3Int(-370,312,876),
				new Vector3Int(604,501,-272),
				new Vector3Int(0,-116,153),
				new Vector3Int(627,-517,-719),
				new Vector3Int(446,378,612),
				new Vector3Int(737,580,-314),
				new Vector3Int(613,348,563),
				new Vector3Int(597,533,-349),
			},
			new List<Vector3Int>
			{
				new Vector3Int(394,-586,-661),
				new Vector3Int(393,-558,-601),
				new Vector3Int(-689,562,-354),
				new Vector3Int(684,225,-798),
				new Vector3Int(-114,-56,111),
				new Vector3Int(-739,-893,597),
				new Vector3Int(-565,-643,-577),
				new Vector3Int(-861,-971,590),
				new Vector3Int(-704,490,533),
				new Vector3Int(-17,-119,-35),
				new Vector3Int(361,-493,-652),
				new Vector3Int(-796,-915,662),
				new Vector3Int(507,388,760),
				new Vector3Int(587,236,-640),
				new Vector3Int(-534,-597,-598),
				new Vector3Int(651,391,763),
				new Vector3Int(385,-536,502),
				new Vector3Int(428,-570,366),
				new Vector3Int(-499,540,-276),
				new Vector3Int(-597,579,-433),
				new Vector3Int(470,-427,432),
				new Vector3Int(726,317,762),
				new Vector3Int(-683,546,506),
				new Vector3Int(602,279,-643),
				new Vector3Int(-526,-685,-624),
				new Vector3Int(-701,405,437),
			},
			new List<Vector3Int>
			{
				new Vector3Int(-399,844,418),
				new Vector3Int(-722,-795,-540),
				new Vector3Int(627,-452,885),
				new Vector3Int(-443,829,419),
				new Vector3Int(-747,-451,528),
				new Vector3Int(556,-356,812),
				new Vector3Int(408,573,-471),
				new Vector3Int(633,433,707),
				new Vector3Int(-620,317,-769),
				new Vector3Int(547,404,848),
				new Vector3Int(757,-880,-722),
				new Vector3Int(-754,-872,-547),
				new Vector3Int(364,498,-597),
				new Vector3Int(487,536,-630),
				new Vector3Int(-61,48,31),
				new Vector3Int(643,-435,791),
				new Vector3Int(-764,-388,544),
				new Vector3Int(-152,-77,-70),
				new Vector3Int(-583,-430,572),
				new Vector3Int(-675,-830,-680),
				new Vector3Int(-472,893,309),
				new Vector3Int(-634,319,-814),
				new Vector3Int(613,-829,-680),
				new Vector3Int(733,-852,-708),
				new Vector3Int(-486,380,-859),
				new Vector3Int(567,444,728),
			},
			new List<Vector3Int>
			{
				new Vector3Int(-477,-923,-562),
				new Vector3Int(-779,-536,360),
				new Vector3Int(-459,-972,-443),
				new Vector3Int(-795,-519,454),
				new Vector3Int(320,809,936),
				new Vector3Int(419,279,-638),
				new Vector3Int(-646,-969,-505),
				new Vector3Int(476,281,-604),
				new Vector3Int(-785,349,538),
				new Vector3Int(18,6,-41),
				new Vector3Int(-699,320,397),
				new Vector3Int(-708,-440,433),
				new Vector3Int(247,760,827),
				new Vector3Int(469,-915,-835),
				new Vector3Int(553,-964,-787),
				new Vector3Int(-719,360,517),
				new Vector3Int(291,745,826),
				new Vector3Int(-603,790,-602),
				new Vector3Int(-579,787,-649),
				new Vector3Int(534,239,-691),
				new Vector3Int(-54,-171,71),
				new Vector3Int(-536,693,-708),
				new Vector3Int(324,-938,-747),
				new Vector3Int(431,-514,673),
				new Vector3Int(612,-472,701),
				new Vector3Int(595,-535,717),
			},
			new List<Vector3Int>
			{
				new Vector3Int(-476,-681,-773),
				new Vector3Int(501,743,-516),
				new Vector3Int(553,756,508),
				new Vector3Int(663,-627,790),
				new Vector3Int(683,-639,610),
				new Vector3Int(-522,743,510),
				new Vector3Int(-653,-618,565),
				new Vector3Int(479,763,518),
				new Vector3Int(-460,346,-739),
				new Vector3Int(451,791,547),
				new Vector3Int(346,-660,-580),
				new Vector3Int(694,-649,820),
				new Vector3Int(-426,482,-782),
				new Vector3Int(427,742,-441),
				new Vector3Int(-559,612,389),
				new Vector3Int(389,-663,-565),
				new Vector3Int(498,740,-660),
				new Vector3Int(-448,-621,-774),
				new Vector3Int(-317,412,-697),
				new Vector3Int(36,-9,7),
				new Vector3Int(-760,-691,589),
				new Vector3Int(-472,578,535),
				new Vector3Int(335,-818,-483),
				new Vector3Int(-565,-684,558),
				new Vector3Int(-383,-672,-806),
			},
			new List<Vector3Int>
			{
				new Vector3Int(410,-606,803),
				new Vector3Int(449,-844,814),
				new Vector3Int(383,-848,778),
				new Vector3Int(-484,796,561),
				new Vector3Int(-728,-606,-672),
				new Vector3Int(-741,469,-674),
				new Vector3Int(-478,816,785),
				new Vector3Int(-597,-643,-793),
				new Vector3Int(-59,1,97),
				new Vector3Int(-776,367,-635),
				new Vector3Int(655,637,540),
				new Vector3Int(706,-844,-593),
				new Vector3Int(6,-117,-25),
				new Vector3Int(642,579,-392),
				new Vector3Int(654,599,-572),
				new Vector3Int(664,610,-362),
				new Vector3Int(621,578,662),
				new Vector3Int(699,-649,-624),
				new Vector3Int(-664,-391,802),
				new Vector3Int(-725,-496,900),
				new Vector3Int(-563,-556,-647),
				new Vector3Int(-679,-428,850),
				new Vector3Int(-748,395,-664),
				new Vector3Int(687,732,647),
				new Vector3Int(-481,753,754),
				new Vector3Int(633,-737,-617),
			},
			new List<Vector3Int>
			{
				new Vector3Int(646,-480,-717),
				new Vector3Int(-770,505,551),
				new Vector3Int(913,318,447),
				new Vector3Int(448,812,-365),
				new Vector3Int(-560,667,-499),
				new Vector3Int(-564,-524,-734),
				new Vector3Int(-680,-549,-697),
				new Vector3Int(851,263,498),
				new Vector3Int(-601,-500,792),
				new Vector3Int(417,-462,-695),
				new Vector3Int(-617,487,-479),
				new Vector3Int(-775,481,761),
				new Vector3Int(549,-537,-665),
				new Vector3Int(437,744,-417),
				new Vector3Int(848,-770,371),
				new Vector3Int(-572,-472,820),
				new Vector3Int(-607,682,-434),
				new Vector3Int(401,722,-491),
				new Vector3Int(790,326,600),
				new Vector3Int(817,-767,434),
				new Vector3Int(163,39,-44),
				new Vector3Int(1,-5,58),
				new Vector3Int(-611,-507,718),
				new Vector3Int(-627,-388,-661),
				new Vector3Int(-715,510,579),
				new Vector3Int(624,-764,391),
			},
			new List<Vector3Int>
			{
				new Vector3Int(311,670,-775),
				new Vector3Int(712,-733,566),
				new Vector3Int(-560,-681,-278),
				new Vector3Int(-51,60,110),
				new Vector3Int(-700,-659,-296),
				new Vector3Int(641,-514,568),
				new Vector3Int(624,694,891),
				new Vector3Int(470,-503,-644),
				new Vector3Int(-551,763,738),
				new Vector3Int(-493,-576,630),
				new Vector3Int(483,676,822),
				new Vector3Int(301,-451,-683),
				new Vector3Int(539,697,795),
				new Vector3Int(416,-448,-753),
				new Vector3Int(-527,-616,736),
				new Vector3Int(-627,-717,-307),
				new Vector3Int(-601,-532,638),
				new Vector3Int(-815,661,-730),
				new Vector3Int(-478,691,870),
				new Vector3Int(-750,658,-716),
				new Vector3Int(446,748,-705),
				new Vector3Int(-967,689,-763),
				new Vector3Int(638,-658,565),
				new Vector3Int(-478,615,676),
				new Vector3Int(428,597,-769),
			},
			new List<Vector3Int>
			{
				new Vector3Int(624,622,-621),
				new Vector3Int(649,590,-567),
				new Vector3Int(10,57,64),
				new Vector3Int(394,-674,-779),
				new Vector3Int(693,781,830),
				new Vector3Int(-556,369,-750),
				new Vector3Int(-590,410,496),
				new Vector3Int(-648,-683,518),
				new Vector3Int(-560,389,-781),
				new Vector3Int(-29,-77,-73),
				new Vector3Int(856,-788,330),
				new Vector3Int(-500,371,598),
				new Vector3Int(-638,-830,499),
				new Vector3Int(366,-787,-652),
				new Vector3Int(798,-701,389),
				new Vector3Int(635,617,-580),
				new Vector3Int(357,-731,-681),
				new Vector3Int(-345,-433,-622),
				new Vector3Int(938,-757,320),
				new Vector3Int(-411,-544,-661),
				new Vector3Int(-579,491,615),
				new Vector3Int(141,50,-115),
				new Vector3Int(-348,-487,-704),
				new Vector3Int(521,745,757),
				new Vector3Int(491,729,859),
				new Vector3Int(-704,-823,603),
				new Vector3Int(-533,382,-904),
			},
			new List<Vector3Int>
			{
				new Vector3Int(-263,581,-415),
				new Vector3Int(-281,390,-455),
				new Vector3Int(916,-537,-806),
				new Vector3Int(-605,382,414),
				new Vector3Int(837,-450,-785),
				new Vector3Int(820,-466,-879),
				new Vector3Int(392,-336,728),
				new Vector3Int(-632,-350,636),
				new Vector3Int(-594,-512,597),
				new Vector3Int(-499,-497,639),
				new Vector3Int(65,78,-18),
				new Vector3Int(-707,462,345),
				new Vector3Int(789,668,-384),
				new Vector3Int(684,328,667),
				new Vector3Int(-644,442,525),
				new Vector3Int(-232,625,-454),
				new Vector3Int(552,397,728),
				new Vector3Int(-450,-675,-592),
				new Vector3Int(876,640,-488),
				new Vector3Int(-494,-610,-772),
				new Vector3Int(719,346,687),
				new Vector3Int(420,-360,714),
				new Vector3Int(-533,-664,-745),
				new Vector3Int(443,-480,686),
				new Vector3Int(758,684,-529),
			},
			new List<Vector3Int>
			{
				new Vector3Int(874,677,-708),
				new Vector3Int(-575,-438,574),
				new Vector3Int(673,661,488),
				new Vector3Int(94,-32,-47),
				new Vector3Int(-444,467,-486),
				new Vector3Int(-64,28,70),
				new Vector3Int(-445,392,-412),
				new Vector3Int(-775,648,332),
				new Vector3Int(760,-684,787),
				new Vector3Int(-705,733,441),
				new Vector3Int(557,-393,-495),
				new Vector3Int(735,590,500),
				new Vector3Int(772,-816,735),
				new Vector3Int(-396,-596,-436),
				new Vector3Int(581,-398,-705),
				new Vector3Int(843,-843,784),
				new Vector3Int(765,622,-654),
				new Vector3Int(-431,-557,-538),
				new Vector3Int(804,674,-823),
				new Vector3Int(-444,-559,537),
				new Vector3Int(-447,611,-483),
				new Vector3Int(586,-453,-677),
				new Vector3Int(-841,690,475),
				new Vector3Int(-609,-584,487),
				new Vector3Int(-424,-567,-464),
				new Vector3Int(676,610,517),
			},
			new List<Vector3Int>
			{
				new Vector3Int(-457,602,-521),
				new Vector3Int(-785,-705,788),
				new Vector3Int(287,-452,868),
				new Vector3Int(235,664,699),
				new Vector3Int(408,-468,-355),
				new Vector3Int(-439,580,-661),
				new Vector3Int(-747,-785,826),
				new Vector3Int(298,-656,818),
				new Vector3Int(-559,876,501),
				new Vector3Int(477,729,-620),
				new Vector3Int(-108,152,68),
				new Vector3Int(-783,-472,-590),
				new Vector3Int(-798,-425,-655),
				new Vector3Int(291,702,828),
				new Vector3Int(433,-352,-392),
				new Vector3Int(272,-405,-428),
				new Vector3Int(240,704,825),
				new Vector3Int(-468,544,-594),
				new Vector3Int(245,-536,845),
				new Vector3Int(-488,821,519),
				new Vector3Int(575,645,-694),
				new Vector3Int(-689,-747,941),
				new Vector3Int(469,685,-728),
				new Vector3Int(-477,793,620),
				new Vector3Int(-603,-415,-610),
				new Vector3Int(14,11,7),
			},
			new List<Vector3Int>
			{
				new Vector3Int(788,552,-857),
				new Vector3Int(-660,-556,795),
				new Vector3Int(-557,617,662),
				new Vector3Int(-25,-55,-84),
				new Vector3Int(794,776,-877),
				new Vector3Int(524,-673,-456),
				new Vector3Int(157,-18,13),
				new Vector3Int(531,-683,-530),
				new Vector3Int(-302,718,-648),
				new Vector3Int(547,891,435),
				new Vector3Int(-481,-842,-697),
				new Vector3Int(675,-802,582),
				new Vector3Int(-294,841,-801),
				new Vector3Int(-743,-397,792),
				new Vector3Int(544,-681,545),
				new Vector3Int(-672,-424,743),
				new Vector3Int(608,793,330),
				new Vector3Int(-486,-787,-669),
				new Vector3Int(-239,779,-750),
				new Vector3Int(791,745,-814),
				new Vector3Int(593,841,367),
				new Vector3Int(709,-656,495),
				new Vector3Int(-530,631,649),
				new Vector3Int(-457,-847,-487),
				new Vector3Int(-525,642,608),
				new Vector3Int(496,-644,-428),
			},
			new List<Vector3Int>
			{
				new Vector3Int(-238,548,843),
				new Vector3Int(842,-440,418),
				new Vector3Int(-285,454,751),
				new Vector3Int(799,419,-559),
				new Vector3Int(719,-870,-801),
				new Vector3Int(740,536,579),
				new Vector3Int(807,491,-376),
				new Vector3Int(-549,300,-752),
				new Vector3Int(-661,-859,519),
				new Vector3Int(811,577,-506),
				new Vector3Int(-686,-675,576),
				new Vector3Int(908,-476,414),
				new Vector3Int(626,541,737),
				new Vector3Int(741,-825,-767),
				new Vector3Int(881,-451,656),
				new Vector3Int(677,411,619),
				new Vector3Int(-621,-612,-426),
				new Vector3Int(32,40,20),
				new Vector3Int(-251,628,740),
				new Vector3Int(-587,-669,-313),
				new Vector3Int(-416,346,-677),
				new Vector3Int(675,-673,-842),
				new Vector3Int(-709,-695,-334),
				new Vector3Int(-715,-764,472),
				new Vector3Int(184,-69,70),
				new Vector3Int(-550,391,-735),
			},
			new List<Vector3Int>
			{
				new Vector3Int(488,654,558),
				new Vector3Int(451,888,-733),
				new Vector3Int(483,888,-660),
				new Vector3Int(-798,636,-780),
				new Vector3Int(-717,497,622),
				new Vector3Int(-643,572,623),
				new Vector3Int(-665,-462,643),
				new Vector3Int(596,698,550),
				new Vector3Int(481,-464,-423),
				new Vector3Int(-3,6,-186),
				new Vector3Int(522,614,452),
				new Vector3Int(438,-558,-488),
				new Vector3Int(-693,-491,671),
				new Vector3Int(-461,-554,-759),
				new Vector3Int(-591,-452,636),
				new Vector3Int(639,871,-733),
				new Vector3Int(-755,621,-783),
				new Vector3Int(824,-383,594),
				new Vector3Int(105,51,-18),
				new Vector3Int(861,-499,619),
				new Vector3Int(-809,539,-938),
				new Vector3Int(-646,716,620),
				new Vector3Int(-481,-734,-747),
				new Vector3Int(410,-375,-562),
				new Vector3Int(788,-436,755),
				new Vector3Int(-450,-764,-769),
			},
			new List<Vector3Int>
			{
				new Vector3Int(-492,308,450),
				new Vector3Int(-319,346,369),
				new Vector3Int(-389,604,-841),
				new Vector3Int(627,687,-533),
				new Vector3Int(768,-378,-808),
				new Vector3Int(-443,346,356),
				new Vector3Int(661,279,514),
				new Vector3Int(-519,-788,-739),
				new Vector3Int(652,694,-527),
				new Vector3Int(-688,-731,607),
				new Vector3Int(481,-473,512),
				new Vector3Int(457,-735,508),
				new Vector3Int(750,-467,-853),
				new Vector3Int(762,295,510),
				new Vector3Int(772,-434,-805),
				new Vector3Int(81,-10,87),
				new Vector3Int(-79,-146,66),
				new Vector3Int(-549,-650,-816),
				new Vector3Int(517,-627,495),
				new Vector3Int(621,252,457),
				new Vector3Int(-586,-705,-699),
				new Vector3Int(-628,647,-834),
				new Vector3Int(-538,474,-847),
				new Vector3Int(650,678,-718),
				new Vector3Int(-588,-751,539),
				new Vector3Int(-629,-624,545),
			},
			new List<Vector3Int>
			{
				new Vector3Int(816,750,750),
				new Vector3Int(413,-797,-714),
				new Vector3Int(-683,-851,-281),
				new Vector3Int(-915,736,410),
				new Vector3Int(673,816,792),
				new Vector3Int(-820,700,413),
				new Vector3Int(661,584,-482),
				new Vector3Int(-723,-943,-352),
				new Vector3Int(361,-785,-582),
				new Vector3Int(660,-698,763),
				new Vector3Int(-499,-562,697),
				new Vector3Int(631,-701,712),
				new Vector3Int(-700,-876,-425),
				new Vector3Int(574,550,-336),
				new Vector3Int(-387,-534,674),
				new Vector3Int(-855,686,385),
				new Vector3Int(648,-760,692),
				new Vector3Int(-502,464,-636),
				new Vector3Int(611,419,-457),
				new Vector3Int(-568,-549,721),
				new Vector3Int(-52,-100,132),
				new Vector3Int(-401,404,-659),
				new Vector3Int(364,-868,-741),
				new Vector3Int(707,680,835),
				new Vector3Int(-129,-24,-46),
				new Vector3Int(-438,456,-592),
			},
			new List<Vector3Int>
			{
				new Vector3Int(-664,626,764),
				new Vector3Int(388,-888,-500),
				new Vector3Int(-675,507,845),
				new Vector3Int(549,479,-672),
				new Vector3Int(539,-610,633),
				new Vector3Int(-14,70,75),
				new Vector3Int(-708,742,-850),
				new Vector3Int(-622,-839,-781),
				new Vector3Int(-112,-80,-29),
				new Vector3Int(-618,-893,873),
				new Vector3Int(-863,654,-829),
				new Vector3Int(-645,-770,866),
				new Vector3Int(756,494,-685),
				new Vector3Int(550,-569,638),
				new Vector3Int(-667,-779,-675),
				new Vector3Int(-771,737,-775),
				new Vector3Int(564,-556,665),
				new Vector3Int(290,-783,-439),
				new Vector3Int(-641,-739,886),
				new Vector3Int(340,-851,-485),
				new Vector3Int(-585,609,884),
				new Vector3Int(406,870,526),
				new Vector3Int(614,566,-711),
				new Vector3Int(-495,-839,-657),
				new Vector3Int(436,768,510),
				new Vector3Int(608,841,474),
			},
			new List<Vector3Int>
			{
				new Vector3Int(492,-446,-368),
				new Vector3Int(-847,-473,896),
				new Vector3Int(-686,-645,-471),
				new Vector3Int(602,-574,731),
				new Vector3Int(-895,-458,859),
				new Vector3Int(-621,-684,-558),
				new Vector3Int(-696,660,618),
				new Vector3Int(370,807,483),
				new Vector3Int(-829,553,-862),
				new Vector3Int(343,878,434),
				new Vector3Int(425,-394,-331),
				new Vector3Int(448,-454,-432),
				new Vector3Int(-60,18,-53),
				new Vector3Int(-671,692,564),
				new Vector3Int(-479,-658,-486),
				new Vector3Int(502,795,431),
				new Vector3Int(-627,501,-833),
				new Vector3Int(732,953,-653),
				new Vector3Int(659,886,-630),
				new Vector3Int(-685,665,577),
				new Vector3Int(-945,-412,791),
				new Vector3Int(748,-540,644),
				new Vector3Int(-13,142,95),
				new Vector3Int(803,861,-613),
				new Vector3Int(627,-409,681),
				new Vector3Int(-799,537,-760),
			},
			new List<Vector3Int>
			{
				new Vector3Int(584,-913,-621),
				new Vector3Int(-669,585,723),
				new Vector3Int(-590,347,-711),
				new Vector3Int(81,-101,14),
				new Vector3Int(387,-872,-577),
				new Vector3Int(-791,-634,-757),
				new Vector3Int(781,343,-388),
				new Vector3Int(-534,-464,686),
				new Vector3Int(853,763,674),
				new Vector3Int(-621,542,-704),
				new Vector3Int(432,-838,-555),
				new Vector3Int(806,585,-415),
				new Vector3Int(437,-582,474),
				new Vector3Int(817,347,-454),
				new Vector3Int(-28,1,-89),
				new Vector3Int(-644,635,558),
				new Vector3Int(-798,-860,-702),
				new Vector3Int(-778,-704,-654),
				new Vector3Int(903,695,720),
				new Vector3Int(-614,454,-638),
				new Vector3Int(-467,-427,738),
				new Vector3Int(-490,-528,678),
				new Vector3Int(805,800,659),
				new Vector3Int(644,-567,487),
				new Vector3Int(579,-654,448),
				new Vector3Int(-790,548,592),
			},
			new List<Vector3Int>
			{
				new Vector3Int(423,-847,767),
				new Vector3Int(653,-771,-637),
				new Vector3Int(-845,-744,-484),
				new Vector3Int(-806,746,-548),
				new Vector3Int(-152,-123,-153),
				new Vector3Int(278,-860,634),
				new Vector3Int(599,-943,-691),
				new Vector3Int(-531,-694,753),
				new Vector3Int(-48,45,-50),
				new Vector3Int(-695,665,561),
				new Vector3Int(-848,-800,-459),
				new Vector3Int(408,551,311),
				new Vector3Int(328,488,445),
				new Vector3Int(377,609,404),
				new Vector3Int(471,338,-463),
				new Vector3Int(-669,-733,-421),
				new Vector3Int(-797,706,-651),
				new Vector3Int(-804,650,513),
				new Vector3Int(390,412,-574),
				new Vector3Int(602,-896,-717),
				new Vector3Int(-639,725,-548),
				new Vector3Int(-688,-712,768),
				new Vector3Int(356,-832,677),
				new Vector3Int(435,333,-465),
				new Vector3Int(-537,-796,818),
				new Vector3Int(-750,677,662),
			},
			new List<Vector3Int>
			{
				new Vector3Int(729,503,-479),
				new Vector3Int(-517,-963,373),
				new Vector3Int(-378,-582,-567),
				new Vector3Int(414,-594,-678),
				new Vector3Int(-559,244,-879),
				new Vector3Int(-318,-621,-456),
				new Vector3Int(-695,246,-838),
				new Vector3Int(605,592,-466),
				new Vector3Int(674,-732,730),
				new Vector3Int(728,-693,703),
				new Vector3Int(-542,344,-835),
				new Vector3Int(-561,-994,412),
				new Vector3Int(633,510,507),
				new Vector3Int(-708,521,549),
				new Vector3Int(585,415,424),
				new Vector3Int(-398,-519,-387),
				new Vector3Int(576,-632,-639),
				new Vector3Int(-326,-945,413),
				new Vector3Int(561,-727,-681),
				new Vector3Int(45,-86,-77),
				new Vector3Int(739,-776,550),
				new Vector3Int(-654,543,735),
				new Vector3Int(640,596,-428),
				new Vector3Int(-677,680,596),
				new Vector3Int(611,493,326),
			},
			new List<Vector3Int>
			{
				new Vector3Int(-401,-616,740),
				new Vector3Int(703,-826,599),
				new Vector3Int(-690,-659,-687),
				new Vector3Int(-348,560,910),
				new Vector3Int(624,-868,732),
				new Vector3Int(900,331,-443),
				new Vector3Int(-484,839,-465),
				new Vector3Int(-467,-528,733),
				new Vector3Int(812,423,-526),
				new Vector3Int(-451,630,897),
				new Vector3Int(-717,-417,-662),
				new Vector3Int(778,310,-469),
				new Vector3Int(-651,-421,-698),
				new Vector3Int(892,384,420),
				new Vector3Int(-550,764,-313),
				new Vector3Int(-528,798,-268),
				new Vector3Int(906,-475,-374),
				new Vector3Int(758,321,510),
				new Vector3Int(656,-791,739),
				new Vector3Int(852,-522,-279),
				new Vector3Int(-403,786,917),
				new Vector3Int(877,-579,-463),
				new Vector3Int(-394,-421,709),
				new Vector3Int(744,323,465),
				new Vector3Int(105,-23,64),
			},
			new List<Vector3Int>
			{
				new Vector3Int(-481,471,-306),
				new Vector3Int(562,-724,861),
				new Vector3Int(476,-793,-720),
				new Vector3Int(-785,753,665),
				new Vector3Int(-567,-417,-645),
				new Vector3Int(-655,-633,546),
				new Vector3Int(-620,-523,657),
				new Vector3Int(517,-618,801),
				new Vector3Int(-890,860,687),
				new Vector3Int(566,930,915),
				new Vector3Int(413,-718,-715),
				new Vector3Int(321,-772,-632),
				new Vector3Int(-680,-567,-661),
				new Vector3Int(-501,453,-380),
				new Vector3Int(-607,459,-359),
				new Vector3Int(543,943,761),
				new Vector3Int(509,902,-485),
				new Vector3Int(689,-600,861),
				new Vector3Int(-867,634,707),
				new Vector3Int(-656,-364,571),
				new Vector3Int(-775,-406,-665),
				new Vector3Int(539,923,792),
				new Vector3Int(520,821,-604),
				new Vector3Int(599,941,-587),
				new Vector3Int(-49,148,48),
				new Vector3Int(-146,23,-56),
			},
			new List<Vector3Int>
			{
				new Vector3Int(-573,-366,309),
				new Vector3Int(-619,-298,359),
				new Vector3Int(531,-593,583),
				new Vector3Int(-820,946,424),
				new Vector3Int(276,717,482),
				new Vector3Int(-791,-310,-476),
				new Vector3Int(-156,152,71),
				new Vector3Int(504,-580,569),
				new Vector3Int(268,870,359),
				new Vector3Int(-752,836,-543),
				new Vector3Int(380,600,-403),
				new Vector3Int(-889,872,-507),
				new Vector3Int(213,819,421),
				new Vector3Int(739,-256,-599),
				new Vector3Int(-8,44,-80),
				new Vector3Int(-915,-310,-603),
				new Vector3Int(276,689,-454),
				new Vector3Int(-817,883,455),
				new Vector3Int(499,-539,558),
				new Vector3Int(752,-410,-622),
				new Vector3Int(326,650,-466),
				new Vector3Int(-634,-261,407),
				new Vector3Int(-809,-370,-648),
				new Vector3Int(732,-379,-626),
				new Vector3Int(-814,926,648),
				new Vector3Int(-775,839,-399),
			},
			new List<Vector3Int>
			{
				new Vector3Int(12,9,58),
				new Vector3Int(-528,-741,535),
				new Vector3Int(821,-855,-688),
				new Vector3Int(933,458,730),
				new Vector3Int(829,-738,634),
				new Vector3Int(-489,-657,-824),
				new Vector3Int(774,-596,637),
				new Vector3Int(-391,-745,-874),
				new Vector3Int(-728,646,405),
				new Vector3Int(710,-695,719),
				new Vector3Int(-673,717,423),
				new Vector3Int(-454,748,-617),
				new Vector3Int(-432,766,-757),
				new Vector3Int(895,516,845),
				new Vector3Int(168,-26,-52),
				new Vector3Int(573,407,-751),
				new Vector3Int(597,-899,-676),
				new Vector3Int(563,602,-706),
				new Vector3Int(682,-790,-718),
				new Vector3Int(-645,728,524),
				new Vector3Int(-517,-691,758),
				new Vector3Int(-396,737,-690),
				new Vector3Int(-454,-776,-912),
				new Vector3Int(853,420,692),
				new Vector3Int(-427,-722,713),
				new Vector3Int(620,422,-686),
			},
			new List<Vector3Int>
			{
				new Vector3Int(556,-743,700),
				new Vector3Int(-394,816,770),
				new Vector3Int(723,-794,659),
				new Vector3Int(855,406,-530),
				new Vector3Int(-561,816,749),
				new Vector3Int(171,-11,61),
				new Vector3Int(-352,905,-776),
				new Vector3Int(-259,-559,-443),
				new Vector3Int(-703,-678,317),
				new Vector3Int(714,560,493),
				new Vector3Int(-14,166,-124),
				new Vector3Int(793,-472,-412),
				new Vector3Int(880,-500,-516),
				new Vector3Int(-640,-698,497),
				new Vector3Int(688,-812,639),
				new Vector3Int(762,-456,-557),
				new Vector3Int(523,559,564),
				new Vector3Int(-601,-758,386),
				new Vector3Int(-228,-515,-482),
				new Vector3Int(722,407,-491),
				new Vector3Int(644,516,455),
				new Vector3Int(-277,-750,-483),
				new Vector3Int(874,440,-377),
				new Vector3Int(-531,943,-709),
				new Vector3Int(-403,713,730),
				new Vector3Int(-355,880,-720),
			},
			new List<Vector3Int>
			{
				new Vector3Int(-622,343,490),
				new Vector3Int(489,286,-639),
				new Vector3Int(-98,-108,-3),
				new Vector3Int(-846,381,-702),
				new Vector3Int(-570,362,-701),
				new Vector3Int(521,-425,-554),
				new Vector3Int(674,715,760),
				new Vector3Int(499,570,-635),
				new Vector3Int(-611,-606,-586),
				new Vector3Int(878,700,801),
				new Vector3Int(373,-559,412),
				new Vector3Int(401,-516,490),
				new Vector3Int(538,399,-663),
				new Vector3Int(-616,-819,408),
				new Vector3Int(674,703,893),
				new Vector3Int(437,-375,-403),
				new Vector3Int(-700,-507,-575),
				new Vector3Int(-604,-751,479),
				new Vector3Int(-500,-758,430),
				new Vector3Int(-804,286,539),
				new Vector3Int(386,-426,557),
				new Vector3Int(67,-103,139),
				new Vector3Int(539,-440,-397),
				new Vector3Int(-742,466,-703),
				new Vector3Int(-574,-488,-570),
				new Vector3Int(-660,232,552),
			},
			new List<Vector3Int>
			{
				new Vector3Int(-922,-608,651),
				new Vector3Int(435,-272,-755),
				new Vector3Int(-479,617,-635),
				new Vector3Int(-564,571,507),
				new Vector3Int(322,412,743),
				new Vector3Int(493,-310,-870),
				new Vector3Int(-761,-376,-659),
				new Vector3Int(579,-642,681),
				new Vector3Int(399,-585,661),
				new Vector3Int(254,423,795),
				new Vector3Int(-813,-523,693),
				new Vector3Int(384,-632,726),
				new Vector3Int(429,439,-675),
				new Vector3Int(-121,75,-1),
				new Vector3Int(-658,582,-669),
				new Vector3Int(-619,-431,-662),
				new Vector3Int(317,534,-598),
				new Vector3Int(-468,541,392),
				new Vector3Int(-644,555,-656),
				new Vector3Int(397,-352,-717),
				new Vector3Int(267,500,-631),
				new Vector3Int(-878,-384,663),
				new Vector3Int(-718,-495,-744),
				new Vector3Int(344,376,669),
				new Vector3Int(-478,499,485),
			},
			new List<Vector3Int>
			{
				new Vector3Int(-760,373,-598),
				new Vector3Int(-714,574,383),
				new Vector3Int(-499,-330,-781),
				new Vector3Int(439,-375,-810),
				new Vector3Int(-644,-789,695),
				new Vector3Int(555,520,671),
				new Vector3Int(-641,-372,-687),
				new Vector3Int(323,-754,683),
				new Vector3Int(-705,366,-824),
				new Vector3Int(-570,-494,-743),
				new Vector3Int(-704,-788,667),
				new Vector3Int(453,-347,-845),
				new Vector3Int(389,-712,826),
				new Vector3Int(-68,12,56),
				new Vector3Int(-695,-821,760),
				new Vector3Int(-657,386,-668),
				new Vector3Int(459,-797,717),
				new Vector3Int(683,630,-428),
				new Vector3Int(-641,550,490),
				new Vector3Int(403,-372,-905),
				new Vector3Int(735,697,-391),
				new Vector3Int(-707,543,578),
				new Vector3Int(675,674,-492),
				new Vector3Int(51,-73,-87),
				new Vector3Int(589,374,584),
				new Vector3Int(720,495,621),
			},
			new List<Vector3Int>
			{
				new Vector3Int(580,-390,-635),
				new Vector3Int(524,-463,348),
				new Vector3Int(338,791,497),
				new Vector3Int(684,-341,-642),
				new Vector3Int(-688,-469,-394),
				new Vector3Int(-613,-321,-360),
				new Vector3Int(562,-401,459),
				new Vector3Int(610,659,-372),
				new Vector3Int(-562,-720,487),
				new Vector3Int(-807,357,614),
				new Vector3Int(380,-450,402),
				new Vector3Int(484,664,-391),
				new Vector3Int(-886,390,488),
				new Vector3Int(-717,-495,-378),
				new Vector3Int(-128,-55,-8),
				new Vector3Int(-701,598,-436),
				new Vector3Int(-818,429,597),
				new Vector3Int(614,790,478),
				new Vector3Int(573,675,-490),
				new Vector3Int(-863,636,-370),
				new Vector3Int(442,792,582),
				new Vector3Int(-814,648,-428),
				new Vector3Int(568,-319,-703),
				new Vector3Int(-560,-644,497),
				new Vector3Int(23,63,84),
				new Vector3Int(-638,-687,585),
			}
		};

	private readonly List<Vector3Int> _foundBeacons = _scanners[0].ToList();
	private readonly List<Vector3Int> _scannerLocations = new List<Vector3Int> { new Vector3Int(0, 0, 0) };
	private readonly List<int> _foundScannerIndexes = new List<int> { 0 };

	private void Start()
	{
		_foundBeacons.ForEach(b => Instantiate(_sphere).position = (Vector3) b / 1000.0f);
		StartCoroutine(FindScanners());
	}

	private void Update()
	{
		_azimuthAngleRadians += Time.deltaTime / 2.0f;
		Vector3 viewTarget = _foundBeacons.Aggregate(Vector3.zero, (a, c) => a + (Vector3) c / 1000.0f) / _foundBeacons.Count;
		float furthestBeaconDistance = Mathf.Sqrt(_foundBeacons.Aggregate(0.0f, (a, c) => Mathf.Max(a, (viewTarget - (Vector3) c / 1000.0f).sqrMagnitude)));
		Vector3 furthestBeacon = _foundBeacons.Aggregate((Vector3) _foundBeacons[0], (a, c) => (viewTarget - a / 1000.0f).sqrMagnitude > (viewTarget - (Vector3) c / 1000.0f).sqrMagnitude ? a : c) / 1000.0f;
		float vFovDegrees = _camera.fieldOfView - 5.0f;
		float hFovDegrees = vFovDegrees * Screen.width / Screen.height;
		float targetDistance = Mathf.Max(furthestBeaconDistance / Mathf.Tan(vFovDegrees / 2.0f * Mathf.Deg2Rad),
			furthestBeaconDistance / Mathf.Tan(hFovDegrees / 2.0f * Mathf.Deg2Rad));
		float horizontalDistance = targetDistance * Mathf.Cos(_altitudeAngleDegrees * Mathf.Deg2Rad);
		Vector3 targetPosition = viewTarget + new Vector3(horizontalDistance * Mathf.Cos(_azimuthAngleRadians),
			targetDistance * Mathf.Sin(_altitudeAngleDegrees * Mathf.Deg2Rad), horizontalDistance * Mathf.Sin(_azimuthAngleRadians));

		transform.SetPositionAndRotation(Interpolate(transform.position, targetPosition, 3.0f),
			Interpolate(transform.rotation, Quaternion.LookRotation(viewTarget - transform.position, Vector3.up), 3.0f));
	}

	private IEnumerator FindScanners()
	{
		var watch = new System.Diagnostics.Stopwatch();
		watch.Start();

		var scannerVisualizationLocations = new List<Vector3>();
		var beaconVisualizationLocations = new List<List<Vector3>>();
		var visualizationTransforms = new List<Transform>();
		while (_scanners.Count > _foundScannerIndexes.Count)
		{
			var threads = new List<Thread>();
			List<Vector3Int> transformedScanner = null;
			int winningThreadScannerIndex = -1;
			var cts = new CancellationTokenSource();

			using (var winningThreadLock = new Mutex())
			{
				for (int i = 0, iMax = _scanners.Count; i < iMax; ++i)
				{
					if (_foundScannerIndexes.Contains(i))
					{
						continue;
					}

					using (var threadStarted = new Semaphore(0, 1))
					{
						var thread = new Thread(() =>
						{
							int threadScannerIndex = i;
							CancellationToken token = cts.Token;
							threadStarted.Release();
							if (ScannersOverlap(_foundBeacons, _scanners[threadScannerIndex],
								out Vector3Int scannerLocation, out List<Vector3Int> orientedBeacons, token))
							{
								winningThreadLock.WaitOne();
								if (transformedScanner == null)
								{
									winningThreadScannerIndex = threadScannerIndex;
									_scannerLocations.Add(scannerLocation);
									transformedScanner = orientedBeacons;
								}
								winningThreadLock.ReleaseMutex();
							}
						});
						threads.Add(thread);
						thread.Start();
						threadStarted.WaitOne();
					}
				}
				yield return new WaitWhile(() =>
				{
					winningThreadLock.WaitOne();
					bool isNull = transformedScanner == null;
					winningThreadLock.ReleaseMutex();
					return isNull;
				});
				cts.Cancel();
				yield return new WaitWhile(() => threads.Any(t => t.IsAlive));
				threads.ForEach(t => t.Join());
			}

			beaconVisualizationLocations.Add(new List<Vector3>());
			transformedScanner.ForEach(b0 =>
			{
				if (!_foundBeacons.Contains(b0))
				{
					_foundBeacons.Add(b0);
					beaconVisualizationLocations.Last().Add((Vector3) b0 / 1000.0f);
					visualizationTransforms.Add(Instantiate(_sphere));
					visualizationTransforms.Last().position = beaconVisualizationLocations.Last().Last();
				}
			});
			_foundScannerIndexes.Add(winningThreadScannerIndex);
			scannerVisualizationLocations.Add((Vector3) _scannerLocations.Last() / 1000.0f);
			visualizationTransforms.Add(Instantiate(_cube));
			visualizationTransforms.Last().position = scannerVisualizationLocations.Last();
			Debug.Log($"found scanner index {winningThreadScannerIndex} @ {_scannerLocations.Last()}");
			Debug.Log($"{_scanners.Count - _foundScannerIndexes.Count} scanners remaining...");
		}

		watch.Stop();

		Debug.Log($"Number of beacons: {_foundBeacons.Count}"); // Part A

		int distance = 0;

		foreach (Vector3Int scannerA in _scannerLocations)
		{
			foreach (Vector3Int scannerB in _scannerLocations)
			{
				distance = Mathf.Max(distance,
					Mathf.Abs(scannerB.x - scannerA.x) + Mathf.Abs(scannerB.y - scannerA.y) + Mathf.Abs(scannerB.z - scannerA.z));
			}
		}

		Debug.Log($"Largest Manhattan distance: {distance}"); // Part B

		Debug.Log($"Took {watch.ElapsedMilliseconds / 1000.0f} Seconds");

		while (true)
		{
			while (!Input.GetKeyDown(KeyCode.R))
			{
				yield return null;
			}
			visualizationTransforms.ForEach(t => Destroy(t.gameObject));
			visualizationTransforms.Clear();
			while (!Input.GetKeyDown(KeyCode.Space))
			{
				yield return null;
			}
			for (int i = 0, iMax = scannerVisualizationLocations.Count; i < iMax; ++i)
			{
				yield return new WaitForSeconds(0.3f);
				visualizationTransforms.Add(Instantiate(_cube));
				visualizationTransforms.Last().position = scannerVisualizationLocations[i];
				beaconVisualizationLocations[i].ForEach(b =>
				{
					visualizationTransforms.Add(Instantiate(_sphere));
					visualizationTransforms.Last().position = b;
				});
			}
		}
	}

	private static bool ScannersOverlap(List<Vector3Int> lhs, List<Vector3Int> rhs,
		out Vector3Int scannerOffset, out List<Vector3Int> orientedBeacons, CancellationToken token)
	{
		scannerOffset = default;
		orientedBeacons = null;
		var rhsBases = transformations.Select(t => rhs.Select(r => t(r)).ToList()).ToList();
		for (int i = 0, iMax = rhsBases.Count; i < iMax; ++i) {
			if (token.IsCancellationRequested)
			{
				return false;
			}
			List<(int, int)> overlaps = GetOverlappingBeaconIndexes(lhs, rhsBases[i]);
			if (overlaps.Count >= 12)
			{
				Vector3Int offset = lhs[overlaps[0].Item1] - rhsBases[i][overlaps[0].Item2];
				orientedBeacons = rhsBases[i].Select(b => b + offset).ToList();
				scannerOffset = offset;
				return true;
			}
		}
		return false;
	}

	private static List<(int, int)> GetOverlappingBeaconIndexes(List<Vector3Int> lhs, List<Vector3Int> rhs)
	{
		var lDiffs = new Dictionary<Vector3Int, (int, int)>();
		for (int lhsI = 0; lhsI < lhs.Count; ++lhsI)
		{
			for (int lhsJ = 0; lhsJ < lhs.Count; ++lhsJ)
			{
				if (lhsI != lhsJ)
				{
					Vector3Int lDiff = lhs[lhsJ] - lhs[lhsI];
					if (Mathf.Abs(lDiff.x) > 2000 || Mathf.Abs(lDiff.y) > 2000 || Mathf.Abs(lDiff.z) > 2000)
					{
						continue;
					}
					lDiffs.Add(lDiff, (lhsI, lhsJ));
				}
			}
		}

		var result = new List<(int, int)>();
		for (int rhsI = 0; rhsI < rhs.Count; ++rhsI)
		{
			if (result.Any(r => r.Item2 == rhsI))
			{
				continue;
			}
			for (int rhsJ = 0; rhsJ < rhs.Count; ++rhsJ)
			{
				if (rhsI != rhsJ)
				{
					Vector3Int rDiff = rhs[rhsJ] - rhs[rhsI];
					if (lDiffs.TryGetValue(rhs[rhsJ] - rhs[rhsI], out (int, int) tuple))
					{
						result.Add((tuple.Item1, rhsI));
						result.Add((tuple.Item2, rhsJ));
					}
				}
			}
		}
		return result;
	}

	private static Vector3 Interpolate(Vector3 current, Vector3 destination, float speed)
	{
		Vector3 distance = destination - current;

		if (distance.sqrMagnitude < 0.000001f)
		{
			return destination;
		}

		Vector3 delta = distance * Mathf.Clamp(Time.deltaTime * speed, 0.0f, 1.0f);

		return current + delta;
	}

	private static Quaternion Interpolate(Quaternion current, Quaternion destination, float speed)
	{
		if (Quaternion.Angle(current, destination) < 0.01f)
		{
			return destination;
		}

		return Quaternion.Slerp(current, destination, speed * Time.deltaTime);
	}
}

#pragma warning restore IDE0044 // Add readonly modifier
#pragma warning restore IDE0051 // Remove unused private members