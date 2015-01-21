using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace WebApplication5.Models.Helpers
{
    public class AppConstants
    {

        public static string BaseSiteUrl
        {
            get
            {
                HttpContext context = HttpContext.Current;
                string baseUrl = context.Request.Url.Scheme + "://" + context.Request.Url.Authority + context.Request.ApplicationPath.TrimEnd('/') + '/';
                return baseUrl;

            }
        }

     

        public static List<string> imenice = new List<string>{"ninja","eunuch","killer","aardvark","actor","actress","adapter","adult","africa","afterthought","airship","albatross","alligator","alloy","anethesiologist","animal","ankle","ant","antelope","apple","appliance","archer","arm","armadillo","armchair","arrow","athlete","atom","aunt","australian","author","baboon","baby","backbone","bacon","badger","bag","bagel","bagpipe","baker","ball","balloon","bamboo","banana","bangle","banjo","banker","baritone","barometer","base","basement","basin","basket","bass","bat","battery","beach","bead","beam","bean","bear","beard","beast","beautician","beauty","beaver","bed","bee","beef","beer","beetle","beggar","beginner","begonia","belgian","bell","belt","bench","berry","bestseller","bicycle","bike","bird","blade","blanket","blinker","block","board","boat","bobcat","body","bolt","bomb","bomber","bone","bongo","bonsai","book","boot","bottle","bow","bowl","box","boy","bra","brain","brake","branch","brandy","bread","breath","brick","broccoli","brochure","broker","brother","brother-in-law","brush","bubble","bucket","building","bull","bulldozer","burglar","bus","bush","butcher","button","buzzard","cabbage","cable","cactus","cake","calculator","calendar","camel","can","cancer","candle","cannon","canoe","captain","car","carbon","card","cardboard","carpenter","carriage","carrot","cart","cartoon","case","cast","cat","caterpillar","cattle","cause","caution","cave","cereal","chain","chair","chalk","character","cheetah","chef","cherry","chest","chick","chicken","chief","chimpanzee","chocolate","christmas","chronometer","church","city","clam","clarinet","clerk","client","clock","closet","cloud","club","clutch","coach","coal","coat","cobweb","cockroach","cocktail","cod","coffee","coil","coin","coke","collar","colony","color","columnist","comb","comic","comma","command","competitor","composer","computer","condor","cone","consonant","continent","cook","cookie","copy","cork","corn","correspondent","cough","cousin","cow","crab","crack","cracker","craftsman","crate","crayfish","crayon","cream","creator","creature","creditor","creek","crib","criminal","crocodile","croissant","crook","crop","cross","crow","crown","cucumber","cultivator","cup","curtain","curve","cushion","custard","customer","cyclone","cylinder","cymbal","daffodil","dahlia","daisy","dancer","daughter","day","death","debtor","deer","dentist","deodorant","desert","desk","dessert","destruction","detective","diamond","dibble","dietician","digger","dime","dimple","dinosaur","dipstick","dirt","disease","dish","distributor","doctor","dog","doll","dollar","dolphin","donkey","door","double","dragon","dragonfly","drain","drake","drawbridge","drawer","dredger","dress","drill","drink","driver","drop","drug","drum","duck","duckling","eagle","ear","earthquake","editor","eel","egg","elbow","elephant","employee","employer","enemy","energy","engine","engineer","error","example","ex-husband","expert","eye","eyebrow","eyelash","eyeliner","face","factory","fairies","fan","fang","farmer","father","father-in-law","faucet","feather","female","fender","fertilizer","fiber","fiberglass","fighter","figure","file","finger","fire","fireman","firewall","fish","fisherman","flag","flame","flare","flax","flower","flute","fly","food","football","footnote","forehead","fork","fountain","fowl","fox","foxglove","frame","freckle","freezer","freighter","fridge","friend","frog","fruit","fur","furniture","galley","gallon","game","garage","garden","garlic","gazelle","gear","ghost","giant","giraffe","girdle","girl","glass","glider","glove","goat","goldfish","gondola","gong","gorilla","gosling","governor","grain","granddaughter","grandfather","grandmother","grandson","grape","grasshopper","great-grandfather","great-grandmother","grenade","guide","guitar","gum","gun","gymnast","hacksaw","half-brother","half-sister","halibut","hamburger","hammer","hamster","hand","handsaw","harbor","harmonica","harp","hat","hawk","head","headlight","headline","heart","helicopter","hell","helmet","herring","hippopotamus","home","hood","hook","horn","horse","hose","hour","hourglass","house","hurricane","hydrant","hyena","ice","icebreaker","ice-cream","icicle","icon","inch","index","ink","insect","instrument","interviewer","island","jacket","jaguar","jam","jar","jasmine","jaw","jeep","jellybean","jellyfish","jet","jewel","joke","judge","juice","jumper","kamikaze","kangaroo","kayak","ketchup","key","keyboard","kick","kidney","kiss","kitchen","kite","kitten","kitty","knee","knife","knight","knot","laborer","lamb","lamp","lan","landmine","laser-beam","latex","lawyer","layer","leaf","leg","lemonade","leopard","letter","lettuce","libra","lier","light","lightning","line","link","lion","lip","lipstick","liquid","liquor","liver","lizard","llama","loaf","lobster","lock","locket","locust","lung","lute","luttuce","lycra","lynx","macaroni","machine","magazine","magician","maid","mailman","male","mallet","manager","map","mask","match","meal","meat","mechanic","medicine","mermaid","metal","microwave","milk","milkshake","mimosa","mind","minibus","mini-skirt","minister","mirror","missile","mistake","moat","modem","mole","mom","monkey","moon","morning","mosquito","mother","mother-in-law","motorboat","motorcycle","mountain","mouse","moustache","mouth","muscle","musician","mustard","nail","name","napkin","neck","needle","nephew","net","network","niece","night","node","noise","noodle","nose","note","notebook","novel","number","nurse","nut","nylon","oak","oatmeal","oboe","octopus","olive","onion","orange","organ","ornament","ostrich","oven","owl","owner","ox","oyster","pajama","palm","pamphlet","pan","pancake","panda","panther","paper","parallelogram","parent","parrot","particle","partner","passenger","pastor","patient","pea","peanut","pear","pediatrician","pelican","pen","pencil","pendulum","perfume","person","pest","pet","pharmacist","pheasant","phone","physician","piano","piccolo","picture","pie","pig","pigeon","pike","pillow","pimple","pin","pine","pint","pipe","pizza","plane","planet","plant","plantation","plate","plier","plow","poet","poison","policeman","politician","popcorn","porcupine","porter","pot","potato","powder","power","priest","printer","product","professor","psychiatrist","puma","pumpkin","puppy","pyjama","pyramid","queen","question","quicksand","quill","rabbit","radar","radiator","radio","radish","raft","rainbow","raincoat","rat","raven","ravioli","ray","recorder","refrigerator","reindeer","relative","religion","relish","reminder","representative","retailer","revolver","rhinoceros","riddle","rifle","ring","river","riverbed","road","robin","rock","rocket","rod","rooster","root","rose","rowboat","rule","sack","sagittarius","sail","sailboat","sailor","salad","salesman","salmon","samurai","sand","sandwich","santa","sardine","sausage","saw","saxophone","scanner","scarecrow","scarf","scissors","scooter","scorpion","scraper","screwdriver","seagull","seal","secretary","servant","server","shadow","shampoo","shark","sheep","sheet","shell","shield","ship","shirt","shoe","shoemaker","shovel","shrimp","singer","single","sink","sister","sister-in-law","skate","skin","skirt","sky","slave","slime","smell","smile","smoke","snail","snake","sneeze","snowflake","snowman","soap","sock","sock-gnome","soda","sofa","softdrink","software","soldier","son","song","sound","soup","soybean","space","spaghetti","sparrow","spear","specialist","speedboat","sphere","sphynx","spider","spike","spinach","spleen","sponge","spoon","spy","square","squid","squirrel","stamp","star","step","step-aunts","step-brothers","stepdaughters","step-daughters","step-fathers","step-grandfathers","step-grandmothers","stepmothers","step-mothers","step-sisters","stepsons","step-sons","step-uncles","stick","stitch","stomach","stone","stool","stopwatch","storm","story","stranger","straw","string","structure","sturgeon","submarine","sugar","suit","sun","sunday","sunflower","surgeon","swamp","swan","sweater","sweatshirt","sword","swordfish","syrup","system","table","tablecloth","tadpole","tail","tailor","tank","target","taxicab","tea","teacher","technician","television","tenor","tent","thing","thread","throat","throne","thumb","thunder","thunderstorm","ticket","tie","tiger","tights","timpani","toad","toast","toe","toenail","toilet","tomato","tongue","toothbrush","tornado","tortellini","tortoise","tower","toy","tractor","train","tramp","tree","triangle","trombone","trout","truck","trunk","tulip","tuna","turkey","turtle","twig","twine","typhoon","umbrella","uncle","undershirt","underwear","vase","vault","vegetable","vegetarian","vessel","veterinarian","vibraphone","violin","visitor","voice","volcano","vulture","waiter","waitress","wallaby","walrus","wasp","watchmaker","weapon","weasel","weather","wedge","weed","weight","whale","wheel","whip","whiskey","whistle","willow","wind","windchime","window","windscreen","windshield","wine","wing","wire","wish","witch","witness","wolf","word","worm","wound","wren","wrench","wrist","writer","xylophone","yak","yarn","yew","yogurt","yoke","zebra","zipper",
            "Aardvark","Abacus","Ache","Airbrush","Alien","Anagram","Angle","Ankle","Alphabet","Antenna","Asphalt","Bacon","Banana","Bangles","Banjo","Bar","Barracuda","Basket","Blizzard","Blunderbuss","Boa","Bog","Broomstick","Bubble","Bug","Bug-a-boo","Bugger","Butter","Cabana","Cake","Calculator","Camera","Candle","Carnival","Carpet","Casino","Cashew","Catfish","Ceiling","Celery","Chalet","Chalk","Chart","Cheddar","Chesterfield","Chicken","Chinchill","Chit-Chat","Chocolate","Chowder","Coal","Compass","Compress","Computer","Conduct","Contents","Cookie","Copper","Corduroy","Cow","Cracker","Crackle","Croissant","Cube","Cupcake","Curtain","Cushion","Cuticle","Daffodil","Delicious","Dictionary","Dimple","Ding-a-ling","Disk","Disco-Duck","Dodo","Dolphin","Dong","Donuts","Dork","Dracula","Duct-Tape","Egad","Elephant","Encasement","Erosion","Eyelash","Feather","Fetish","Finger","Fish","Flame","Flash","Flavour","Flick","Flour","Flower","Fork","Fuse","Fusion","Garlic","Gelatin","Glebe","Glitter","Glossy","Groceries","Goulashes","Guacamole","Gumdrop","Haberdashery","Hamster","Hippopotamus","Hobbit","Hold","Hooligan","Hydrant","Icicles","Idiot","Implode","Implosion","Issue","Jell-O","Jewel","Jump","Kabob","Kasai","Kite","Kiwi","Ketchup","Knob","Laces","Lacy","Laughter","Leaflet","Legacy","Leprechaun","Lollypop","Lumberjack","Macadamia","Magenta","Magic","Mango","Margarine","Massimo","Mechanical","Medicine","Melon","Meow","Mesh","Metric","Microphone","Minnow","Mitten","Mozzarella","Muck","Mustache","Nanimo","Noodle","Nostril","Nuggets","Oatmeal","Oboe","Octopus","Odour","Ointment","Olive","Optic","Overhead","Ox","Oxen","Pajamas","Pancake","Pansy","Paper","Paprika","Parmesan","Pasta","Pattern","Pecan","Peek-a-boo","Pen","Pepper","Pepperoni","Peppermint","Perfume","Periwinkle","Photograph","Pie","Pillow","Pimple","Pineapple","Pistachio","Plush","Polish","Pompom","Poodle","Pop","Popsicle","Prism","Prospector","Prosper","Pudding","Puppet","Puzzle","Queer","Query","Radish","Rainbow","Ribbon","Salami","Sandwich","Saturday","Saturn","Saxophone","Scissors","Scooter","Scrabbleship","Scrunchie","Scuffle","Shadow","Silicone","Smut","Snap","Snooker","Socks","Soya","Spaghett","Spatula","Spiral","Splurge","Spoon","Sprinkle","Square","Squirrel","Stuffing","Sticky","Sugar","Sunshine","Tape","Tat","Teepee","Telephone","Television","Tip","Tofu","Toga","Trestle","Tulip","Turnip","Turtle","Tusks","Unicycle","Unique","Uranus","Vegetable","Waddle","Waffle","Wallet","Walnut","Wagon","Window","Whatever","Wobbly","Yellow","Zap","Zebra","Zigzag","Zip",
};

    public static List<string> pridjevi = new List<string> {"Multiple","The", "Long-range","Unsophisticated", "Crazed", "The Big Bad", "Silver", "Raging", "extreme", "Army of", "Disco", "Master", "Intoxicated", "Heroes and", "Spitting", "Holy", "The Flaming", "Scared", "Paintball", "Pudge and Four", "Kung Fu", "Fire Breathing", "The Mighty", "cunning", "blue", "red", "green", "brown", "yellow", "black", "white", "purple", "pink", "big", "huge", "immense", "enormous", "mammoth", "massive", "vast", "large", "wide", "spacious", "small", "little", "tiny", "high", "long", "tall", "low", "short", "gigantic", "teeny", "weeny", "petite", "scrawny", "happy", "gald", "beatific", "blissful", "cheerful", "chirpy", "content", "pleased", "satisfied", "delighted", "ecstatic", "exalted", "elated", "sad", "bereft", "blue", "broken", "broken-hearted", "broody", "bruised", "broody", "careworn", "deflated", "dark", "demoralised", "depressed", "desolate", "despondent", "disaffected", "disappointed", "disconsolate", "discouraged", "dismal", "disillusioned", "disheartened", "dismayed", "displeased", "dissatisfied", "distressed", "distraught", "doleful",  "downhearted",  "tender", "angry", "excited", "envious", "embarassed", "frightened", "good", "excellent", "brilliant", "splendid", "fantastic", "magnificent", "bad", "terrible", "awsome", "awful", "nice", "beautiful", "pretty", "gorgeous", "cute", "glamorous", "elegant", "good-looking", "handsome", "ugly", "unsightly", "right", "wrong", "funny", "amusing", "entertaining", "light", "heavy", "fat", "stout", "thin", "slim", "clean", "dirty", "filthy", "straight", "noisy", "quiet", "still", "tranquil", "calm", "powerful", "nutritious", "adorable", "adventurous", "aggressive", "alert", "attractive", "bloody", "blushing", "colorful", "exciting", "graceful", "grotesque", "drab", "dull", "homely", "plain", "precious", "sparkling", "fragile", "frail", "weak", "strong", "doubtful", "bewildred", "confused", "puzzled", "cautious", "careful", "concerned", "innocent", "guilty", "crazy", "silly", "stupid", "foolish", "clumsy", "intelligent", "clever", "cunning", "shy", "timid", "rich", "poor", "wild", "defiant", "courageous", "brave", "helpful", "helpless", "unhelpful", "important", "principal", "paramount", "famous", "notorious", "fictitious", "real", "true", "false", "imaginary", "alive", "dead", "odd", "weird", "unsusual", "strange", "outsanding", "impossible", "improbable", "easy", "difficult", "closed", "open", "late", "early", "punctual", "behindhand", "delayed", "ill-timed", "premature", "tardy", "unearthly", "modern", "traditional", "old-fashioned", "young", "old", "new", "slow", "swift", "quick", "rapid", "brief", "short", "long", "wooden", "woolen", "cottony", "fibrous", "metallic", "bronze", "old", "ancient", "aged", "senile", "elderly", "ageless", "oldish", "overage", "young", "juvenile", "adolescent", "teenage", "underage", "youthful",  "delicious", "fresh", "juicy", "ripe", "rotten", "salty", "sour", "spicy", "stale", "sticky", "sweet", "tart", "tasteless", "tasty", "thirsty", "fluttering", "fuzzy", "greasy", "grubby", "hard", "hot", "icy", "loose", "melted", "plastic", "prickly", "rough", "scattered", "shaggy", "shaky", "sharp", "shivering", "silky", "slimy", "slippery", "smooth", "soft", "solid", "steady", "sticky", "tight", "uneven", "weak", "wet", "wooden", "yummy", "boiling", "cooing", "deafening", "faint", "harsh", "high-pitched", "hissing", "hushed", "husky", "loud", "melodic", "moaning", "mute", "noisy", "purring", "quiet", "raspy", "resonant", "screeching", "shrill", "silent", "soft", "squealing", "thundering", "voiceless", "whispering",
    "abnormal","absentminded","absolute","accidental","active","actual","acute","affirmative","agreeable","angry","annoying","annual","anxious","appropriate","arrogant","awful","awkward","bad","bare","bashful","beautiful","believabe","bewildered","bitter","bleak","blind","blissful","bold","boastful","boyish","brave","brief","bright","brilliant","brisk","brutal","busy","calm","candid","careful","careless","casual","cautious","certain","charming","cheerful","childish","clean","clear","clever","clumsy","coincidental","cold","colorful","common","comfortable","compact","compassionate","complete","confused","consistent","constant","continual","continuous","cool","correct","courageous","covert","crazy","cruel","cunning","curious","cute","dangerous","daring","dark","dear","decent","deep","defiant","delicate","delightful","dense","different","diligent","dim","docile","doubtful","dramatical","eager","earnest","easily","efficient","elaborate","eloquent","elegant","energetical","enormous","essential","eternal","exceeding","exceptional","excited","explicit","extraordinary","extreme","faithful","famous","fashionable","fast","fatal","favorable","ferocious","fervent","fierce","fiery","final","fluent","fond","foolish","formal","fortunate","frank","free","furious","generous","genuine","gentle","girlish","gleeful","graceful","gracious","grateful","great","greedy","grim","half-hearted","handsome","happy","harsh","hateful","haunting","healthy","heartly","heavy","helpful","high","honest","hopeless","huge","humorous","hungry","hysterical","icy","idiotical","imaginative","immeasurabe","immediate","immense","impatient","impressive","inappropriate","incorrect","independent","inevitabe","infinite","innocent","inquisitive","instant","intelligent","intense","interesting","invisible","ironical","irrefutable","irritable","jagged","jealous","jovial","joyfull","joyless","joyous","judgmental","just","keen","kindhearted","kind","knavish","knowing","last","lazy","light","lively","loose","loud","loving","loyal","lucky","luxurious","mad","magical","majestical","marked","material","meaningful","mechanical","menacing","mere","merry","methodical","mighty","miserabe","mocking","mortal","mysterious","nasty","natural","naughty","neat","needy","negative","nervous","nice","noisy","normal","numb","obedient","obnoxious","odd","offensive","official","optimistical","ordinary","outdoor","outrageous","overconfident","painful","painless","passionate","patien","perfect","persistent","persuasive","plain","playful","polite","poor","positive","powerful","proud","punctual","quaint","quick","quiet","quirky","random","rapid","rare","real","reasonable","reckless","regular","reliable","reluctant","remarkable","reponsible","resentful","rich","ridiculous","righteous","rightful","rude","ruthless","sad","safe","scary","selfish","selfless","serious","shameless","sharp","sheepish","shoddy","significant","silent","simple","sincere","shy","skillful","sleepy","slow","sly","smooth","spectacular","speedy","spiritual","steady","stern","strict","stupid","stylish","subtle","successful","sudden","sufficient","suitabe","superficial","supreme","surprised","suspicious","sweet","swift","sympathetic","systematic","tender","tense","terrible","thankful","thoughtful","touching","tremendous","true","truthful","ultimate","unbearable","unbelievable","unexpected","unfailing","unfortunate","unimpressive","universal","unnatural","unwilling","urgent","useful","useless","vague","vain","valiant","vicious","victorious","vigilant","violent","visible","visually","warm","weak","weary","wet","wicked","wise","wonderful","worried","wrong","youthful","zealous",
    };


        public static List<Sat> Sats = new List<Sat> {
                new Sat{SatID=1,Opis="-2 h/week"},
                new Sat{SatID=2,Opis="2-6 h/week"},
                new Sat{SatID=6,Opis="6-10 h/week"},
                new Sat{SatID=10,Opis="10-14 h/week"},
                new Sat{SatID=14,Opis="14-18 h/week"},
                new Sat{SatID=18,Opis="18-22 h/week"},
                new Sat{SatID=22,Opis="22-26 h/week"},
                new Sat{SatID=26,Opis="26-30 h/week"},
                new Sat{SatID=30,Opis="30-34 h/week"},
                new Sat{SatID=34,Opis="34-38 h/week"},
                new Sat{SatID=38,Opis="38-42 h/week"},
                new Sat{SatID=42,Opis="42-46 h/week"},
                new Sat{SatID=46,Opis="46-50 h/week"},
                new Sat{SatID=50,Opis="50-54 h/week"},
                new Sat{SatID=54,Opis="54-58 h/week"},
                new Sat{SatID=58,Opis="58+ h/week"},
            };

        public static List<MMR> MMRs = new List<MMR> {
                new MMR{MMRID=2000,Opis="-2500"},
                new MMR{MMRID=2500,Opis="2500-3000"},
                new MMR{MMRID=3000,Opis="3000-3400"},
                new MMR{MMRID=3400,Opis="3400-3600"},
                new MMR{MMRID=3600,Opis="3600-3800"},
                new MMR{MMRID=3800,Opis="3800-4000"},
                new MMR{MMRID=4000,Opis="4000-4200"},
                new MMR{MMRID=4200,Opis="4200-4400"},
                new MMR{MMRID=4400,Opis="4400-4600"},
                new MMR{MMRID=4600,Opis="4600-4800"},
                new MMR{MMRID=4800,Opis="4800-5000"},
                new MMR{MMRID=5000,Opis="5000-5200"},
                new MMR{MMRID=5200,Opis="5200-5400"},
                new MMR{MMRID=5400,Opis="5400-5600"},
                new MMR{MMRID=5600,Opis="5600-5800"},
                new MMR{MMRID=5800,Opis="5800-6000"},
                new MMR{MMRID=6000,Opis="6000+"},
            };

        public static List<Pozicija> pozicije = new List<Pozicija> {
                new Pozicija{PozicijaID=1,Opis="1"},
                new Pozicija{PozicijaID=2,Opis="2"},
                new Pozicija{PozicijaID=3,Opis="3"},
                new Pozicija{PozicijaID=4,Opis="4"},
                new Pozicija{PozicijaID=5,Opis="5"},
       };


        public static List<Searc> Searcs = new List<Searc> {
                new Searc{SearcID=1,Opis="ON"},
                new Searc{SearcID=2,Opis="OFF"},
            };

        public static List<Cilj> Ciljs = new List<Cilj> {
                new Cilj{CiljID=1,Opis="Just FUN - small ambition"},
                new Cilj{CiljID=2,Opis="Leagues, small tournaments - medium ambition"},
                new Cilj{CiljID=3,Opis="Road to pro - maximum ambition"},
            };

        public static List<Mic> Mics = new List<Mic> {
                new Mic{MicID=1,Opis="YES"},
                new Mic{MicID=2,Opis="NO"},
            };

        public static List<TimeZ> timeZones()
        {
            ReadOnlyCollection<TimeZoneInfo> timeZonest = TimeZoneInfo.GetSystemTimeZones();

            List<TimeZ> timeZones = new List<TimeZ>();
            int i = 0;
            timeZones.Add(new TimeZ { TimeZID = i, Opis = "Unknown Time Zone" });
            foreach (TimeZoneInfo timeZoneInfo in timeZonest)
            {
                i++;
                timeZones.Add(new TimeZ { TimeZID = i, Opis = timeZoneInfo.DisplayName });
            }
             return timeZones;
       }

        




        public static List<Godina> godine = new List<Godina> {
                new Godina{GodinaID=1,Opis="0-11"},
                new Godina{GodinaID=2,Opis="12-14"},
                new Godina{GodinaID=3,Opis="15-17"},
                new Godina{GodinaID=4,Opis="18-20"},
                new Godina{GodinaID=5,Opis="21-24"},
                new Godina{GodinaID=6,Opis="25-28"},
                new Godina{GodinaID=7,Opis="29-122y,164 days"},
       };


    }

    public class Sat
    {
        public int SatID { get; set; }
        public string Opis { get; set; }
    }
    public class MMR
    {
        public int MMRID { get; set; }
        public string Opis { get; set; }
    }

    public class Pozicija
    {
        public int PozicijaID { get; set; }
        public string Opis { get; set; }
    }

    public class TimeZ
    {
        public int TimeZID { get; set; }
        public string Opis { get; set; }
    }
    public class Mic
    {
        public int MicID { get; set; }
        public string Opis { get; set; }
    }
    public class Searc
    {
        public int SearcID { get; set; }
        public string Opis { get; set; }
    }
    public class Cilj
    {
        public int CiljID { get; set; }
        public string Opis { get; set; }
    }
    public class Godina
    {
        public int GodinaID { get; set; }
        public string Opis { get; set; }
    }
}