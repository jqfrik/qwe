using ForksWebAPI.Common.Client;
using ForksWebAPI.DATA.Models;
using ForksWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ForksWebAPI.DATA
{
    public class DataInitializer
    {
        public static async Task Seed(ForksDbContext dbContext)
        {
            await dbContext.Database.MigrateAsync();

            if (dbContext.Forks.Any())
            {
                var all = from c in dbContext.Forks select c;
                dbContext.Forks.RemoveRange(all);
                dbContext.SaveChanges();
            }
            if (dbContext.Bet.Any())
            {
                var all = from c in dbContext.Bet select c;
                dbContext.Bet.RemoveRange(all);
                dbContext.SaveChanges();
            }
            if (dbContext.Users.Any())
            {
                var all = from c in dbContext.Users select c;
                dbContext.Users.RemoveRange(all);
                dbContext.SaveChanges();

            }
            if (dbContext.RequestBetDatas.Any())
            {
                var all = from d in dbContext.RequestBetDatas select d;
                dbContext.RequestBetDatas.RemoveRange(all);
                dbContext.SaveChanges();
            }
            if (dbContext.ResponseBetDatas.Any())
            {
                var all = from d in dbContext.ResponseBetDatas select d;
                dbContext.ResponseBetDatas.RemoveRange(all);
                dbContext.SaveChanges();
            }
            if (dbContext.RequestAnotherBets.Any())
            {
                var all = from d in dbContext.RequestAnotherBets select d;
                dbContext.RequestAnotherBets.RemoveRange(all);
                dbContext.SaveChanges();
            }
            if (dbContext.ResponseAnotherBets.Any())
            {
                var all = from d in dbContext.ResponseAnotherBets select d;
                dbContext.ResponseAnotherBets.RemoveRange(all);
                dbContext.SaveChanges();
            }

            string Key = @"a}NnJnE1^nQ~]W45^n]1HPUsI@]p]PNmKG4~KC^iJ6Ri]S]s^C]mKmNHQC<7\RJVJohU^7FaQB5/Wj44JApUU\|MQCJ<\\1BgApE\ihafo1qW\1rJh5PW\ImHGM~^CA|]~c0J}4|^PVoHPU6JPop]SJlJW4s^nNmJ@M4^SNoJnImKmN`Q44s\RJWfopPU\FOU5R=WRg4JApEVi5MQ446WShJgB1EQi1HUAh/WRFKfB1EU\0mHGM5^Pg4^C]0KG5h^@hmHPVmICQp]PA4IG41KCA}ISA1^@gs^nAmKmN`fhA4WS1OfQpPU\FNQ4tkW4g4JAh/^75MQC|p\hJRgB46Qi|afoR=WQFNIAtqVismHGM}KP]6JSQ5IW44^SBmHPViI~cp]PRiJm5i]PQ0]nBi]iNiKCMmKmNHQ45~WQFkfB5UW\FaU5B=\hg4JB1@\i=MQ4NqW\1RgB5P\i=OQAtpW4JJIB1EVPgmHGM1ICQ0JiNlJm4~KSMsHPVn^C]pK@M7^W5nJ@U1J@g1IPRo]PomKmN`boR/WQFJJAt/R\Fafi=~W5g4JB1P\\1MQ45=WRFjgAhtR\5NeRQ|W\1aeR5/eC0mHGM4]iAsI~I0]m47KSBnHPU}Inop]iVi^G44InE~KCNlIPRm^@gmKmNNeR^rWj1nJ4pU\\FOJoo|\hg4JB1UUPRMQ4A5WRFrgAt/f7|OJotk\\1nbohqRi<mHGM5]nQ}^n^nIG5o]Pg|HPU~]PApKPMsJG5l^@]sK@c4Ii]1ICAmKmNOUBNwW5FOfB1PP\FNQA^tW5g4JAt/\P`MQCJ=W5FjgAo6UPFNQ45/WP^WeQo6RPgmHGM|]iQ}J@I7^m5iISQ0HPUs^i]p]nM5]}5mInVoJ@]0^nRnIPMmKmNHQCg7WQFCfB1PP\FHUBQ6Whg4JApE\\1MQ41r\RF`gAh@PPBObhQ5W4JWeB5U]~gmHGM4^CJl^PE0Im5n^nFiHPVm^Pop]nQ5KW40J~hi^SA6]~U1]SUmKmNaUA45W\1RJQhU]7FNUAR<WRg4JB1tWPVMQ4A6W4JVgAtPRi1OQCg|WhFCfotqQi4mHGNoK@o}I6A~KW4~JPg}HPU7]PQpKCA6J}47JPEs]PRlJPE5JnUmKmN`Q51q\RFSeAo6P\FOQBNkWhg4JB5/Ui=MQ4B/WP^VgAt/]7JHUBB<WQFJJB1PQi4mHGNlI~U4]~A|Jm4}JSBmHPU0]6ApK@g0JG5lIC^oI@Q5^SIsK@AmKmNNQC|qW4FjJAhtQ\FOQAQ4\og4JB5Uf71MQCI4\S1RgB5UR\|NfoA7W4JVJohUWismHGM1I~E7JP^hI}45K@g6HPVhJPop]PhiIG45KPM5]P`iJ6]5K@QmKmNNbo47WS1rJB1UU\FNbh^kWQg4JAhq]~VMQ45=\RFBgAp@QPBOfiI4W5JKfopUV\0mHGM}JPFn]Pc0Im4}ICM}HPVl]~opKPJiI}4}^PRn]SRm]n`h^nAmKmNNfi<4Wj1kfot/V\FObo^rWj44JAhEUilMQC<4WhJFgAp@QilOU445\oJJJh5PQiomHGM|^i]sK@g0JG45JSQ~HPU0I~EpKPo7IG44]PAsKPc7IP`iJnImKmNaUCJ/\oFjJQt@U\FNeRNqWi44JB1P\PFMQC|pW\1RgAtEPP`HQCI4Wj1KfQpERPAmHGNl^n]4JPQ7IG5mISQsHPU}^iApK@c}JW46^PA6^PRn]Pg5J6ImKmNNfoh~\S1Wf55tW\FHU5^k\Rg4JAh/Ui=MQ447\P^FgAhPRihNfig4\i1GfAhEWPAmHGNiJPE0JnQ5]m5nKS]4HPVo]SUpKPli^m45I6A4]~MsJ6Ms]PomKmN`filrWj1nbh46Q\F`bhQ|W5g4JB1EW\JMQ45/\RJFgAtEQi|OQAQ|WRFaeR1qQPUmHGNnK@I~^nI5]W41JPhnHPU~IPUp]i]1]}45]iRiJS^l]SU7^ngmKmNOQB^w\\1jJApE]7FOJoA|W4g4JB5/^~FMQ41qWhFBgB5P\\|aeQo6WoFnJ55UQPEmHGM4JiU5^SI}Jm4sKSM~HPU4JiIpKPU1^G47Ii^lKCRhICU6^SQmKmN`bhB<\hFNJApUV\F`biJ=Whg4JB5PPPBMQClrWQFFgAtE\P^OQC|qW4JRJot/fComHGNiK@Us]~`nJW45ISU~HPU|^@gp]SIsIW41InE6J~hh^@U~^nUmKmNHUCFwW\1RJ4t/P\F`JhA5\Rg4JAt/QilMQ45=WoFFgApPQ\JObiI7W4FjbQpUU\smHGNoInlmKSNnKG5hIPE~HPU~^S]p]SNm^W4~]nQ0JCU|^P]7I~ImKmNaQA5<\RFWfB1U\\FNU55~\og4JB1EeC5MQ4B=W\1VgAt@PP^Nbi<7WhJFJQk6Wi<mHGMs]~osKPg5Jm5nJ~FhHPU5J6ApKSM~Im41ICBiJPg1^PM|]SAmKmNHUBNtWj1keAtEf7F`QBNpWRg4JB5Uf71MQCJ<WQJVgB5PQPV`Q5B=\i1`JR1PQPEmHGM4JC^n]6I5IW4sJnQ0HPVn]~Qp]S^hI}5nJ@FoI@BlJiQ6ISQmKmNOUClq\RFwfB5tR\F`eQos\hg4JB1PRi5MQ4B=\RFFgAhE]~VNfi<7\ShVIB5q]CsmHGM}JCVoIPI1IG41ICU5HPU0]~Up]SMs]}5hI@FnJ~lhJ6Q7]SMmKmNNQ445Wj1NJopU^7FHUB47\Rg4JB1@WilMQ4A|W\1jgAt@RP`aQ41t\hJOf4h@RicmHGM}KP]1I~M|Jm4|IC]7HPU7JiQp]SA}J}41JSA7^P]~ICQ6JnUmKmNaUCgsWhJGf4hqU\Fafo5/W5g4JB1PRi1MQCJ/WRF`gAo6Qi|NQA^rWS1cfAp@\icmHGNiIiI5IPQ5]m45IiBiHPU4J@IpK@I4^G44I~liJPJh]SI1]~UmKmN`fhNq\\1NIR1@\\F`Q546\i44JB5qWihMQClpWQFNgAh@P\1HU44s\oFWfR5qRi<mHGNl^@g}IC]|JG4~J@^oHPU|J@cp]SI}IG44^nI|]~Is]6Rm^PQmKmNaU4o4Wj1`bQt/]7FHUA^pWS44JAhtPP^MQ4B=WQFFgB5/Wi|`QAQ7W4FOfQhU\PUmHGNn]nQ~KCAsJm47I6Q0HPU}IiIpKSVlIG5l]nI1^PFmICQ6JPEmKmN`eQ46W5JNbAtPW\FaQClkWRg4JB5/RihMQ4Np\\1ngApE]7JNQ51kWn^KfAttUicmHGM7^CRnI~I0^G5m]nM5HPVl^ncp]iJnIm5lI~U~^SBn^PE0]iImKmNNQC<4\oJSeR5/V\FNfoR<WS44JB5Uf~RMQ4B=WS1VgAtU\i1NU4h=W5FNbQh/W\0mHGNoIiBlJPA1^W45]SU5HPVm^C]pKSRn^m44JnhlJno|]PlhJ@AmKmNOUA4|WQJWfQp@P\FHQAtq\og4JB5PRi=MQ447W5FRgAp@RPVNQBQ|\RJRJ4pERP]mHGNhK@Jl]PFoKW44]PhnHPU5^PcpK@lmIm5n^iNi^SI|^PlmI6ImKmNaU5QsWRJSeR5q]7FNUAh/WS44JAhPRi1MQC|pWRFNgAo6\\1aUAtrWRFNbopE\PEmHGNn]nosIiRlJG4|ICNoHPU~KSApKPo|]}47JCM~KCVoJ~hlI~EmKmN`Q4A7\RJ`IAp@U\F`QBR/Whg4JAh/Pi5MQ4As\oFFgB5q]ClNUA1pWhFNJB1UVP]mHGM|KC]s]S]0Im41J~c6HPU|Ji]pKPBoJW45]6M|IPQ1Inc6]SMmKmNHQB^w\oJ`bB1q]7FHUClkW4g4JAtt\PFMQ44|\\1FgAtqfClOeRNwWS1ceR5PQPgmHGNlInIs^iBi^G4}I@UsHPU0^PopK@o~^W44J~I}J~^hJPNh]S]mKmN`Jotp\S1kfopU^7F`fi|tWhg4JB1PQP^MQC|kWoFJgAhURi5`fo^pWQJNJopUPi4mHGM5I~BlIPM7I}46^PBhHPU5^PQpK@UsKG4s^SM5J@A4IPo~InAmKmNNJhA|Wi1OeB1ER\FNQ55=WRg4JAtq]~`MQClr\\1RgAhtUPVNUAR<W\1Rg4hqRi0mHGM7I@`iJCU~JW5i^PFlHPVh^@gpK@ln]m4|^nU4^@hnJiA7K@ImKmNOU4NrWoFOfohtQ\FNQ55<W\44JB1qPi1MQ45<W5JFgB1PR\5OeR1q\\1RbopEW\4mHGNl^SNnJ6I1]}44JCQ|HPU}JCIpK@g5]}4s^ncsInA1JSA6JnQmKmNOUAQ7WhFOeApUf7FNbh1w\hg4JB5U\\1MQClqWhJVgApP\\5`fiFwW5JNg4pPRismHGM5J6ViJCM0IG5o]PJoHPU6KSIpK@I0IG44JPllJSBiInQ1J@MmKmNaQ45/WQFBbR1tP\FHQCg|\hg4JAhE\\|MQCIsW5FJgAt@Q\JHQ546W4FafQhqW\smHGNnJPVoKCA}]}45I6M4HPU1Ii]p]nJlIG5lInQ6^P`lJSQ1^iMmKmNOQB44W5FjbQhPU\F`eR1p\S44JAttQ\5MQ45~W4JRgB5qRi5HUA1wWP^NJotE]~gmHGM7I~c1J~c}^m5o]~A1HPU1JnQp]nM0J}4|]n^iJiRl^@o|IPImKmNNfiFq\hJVbohqV\FNUC=/\og4JAhUPi1MQ4B<W5FVgAt/V\JObh1rWihFbR5UQi4mHGM}I~FoInE4I}5m]6JoHPU4I~gpKSVoKG5hJPNl]P]}K@U}I6MmKmNOeQ1w\hFCeR1@R\FNeQo4\i44JAtU\i5MQC=~\hJNgB5qUPF`bh44Wn^WeR1@PPQmHGNmJCAsI~VnJW4}^PM5HPU}]~Mp]PBlJ}5mJCA4I~Q~ISU}J@ImKmNOUClqW4JWfQh@P\FOQB^tWS44JAo6U\JMQC<7W5FFgAtE\PRObilk\S1Ofh1UfCcmHGNlICRlJ~I7]W40JPJnHPUsISUpKSQ~KW5oISRmJno~]~E4JiQmKmNNbo1r\P^FIR5/\\FNQ5B~Wog4JAt/f7|MQ4B/Wi1VgAtEPi|NbhR~W\1`JAh/U\ImHGM}JS^o^SA1KW5mK@I5HPVn]nopKP]sJW5h]~FoIPc7ISA6]nomKmNNUA5<\P^Sf4pPQ\FOU5B=\\44JAt/WihMQ44|WoFBgAtE]C5HUA^w\\1OfB5PRPgmHGM~^PI|K@^i]}4s]nQsHPU1J6]p]PA~]W4~^Pg4^no~]nlh^PcmKmNafhQ5WhFNbB1t\\FaQ4Nk\Rg4JAtqfC=MQ4Np\S1VgAtqfC|aU41p\RJNJh5PUPAmHGNiJC]|JiI6JG45K@JhHPVhKSMp]PUs]W4s^nQ0]PRnI~c|]iUmKmN`fiJ~\oJRJQpUW\FaQ4osWhg4JB5UR\5MQ45/\i1ngB5/Qi5afotkWS1Wf4hqQ\smHGM}JiNhIPlnJG5i]nU}HPU~]6QpKSBlKW45JPE~JiVl]Po7J~gmKmNNUCFpW\1VJAhqQ\FNeRA6\\44JB5q^~^MQC<5W4JFgAo6UP^OQClqW5FBbok6RiomHGM~JCRhIno6Im46I~Q}HPU1]nMp]SRoIm5iJ~E4KPlm]6Bm]SAmKmN`bhR<W4FVJR1ER\FOQC<5W\44JAhU\P^MQC|qWShNgB5/^~VaQ4A4W4JRIAtqfComHGM4IiU4^nc5^W45]S]~HPU|JnApK@`l]W4s]nVoJnll]6]}]6AmKmNaUA44\RJWeQhq\\FOfhB/Wi44JApEUPFMQC==WhJVgB1tWPF`fh1r\RFBJ4pUfComHGM|]~^oIi^lIm5iJCA6HPVo^PMp]PM5^m5i]i]6KPhi^Pli^nMmKmNNU5R~W4FafAttQ\F`Jh1k\hg4JAhEVPFMQClpW\1NgB1EQ\|HQAB<\oFFJ51PPiomHGMsK@Q~JS]sIG5mIC^oHPU~^@Up]nQ~KW5mICA7]PU4JSQ4KP]mKmNHUANkW4FOeAt@W\FOQCg4\S44JB1UfC1MQC|k\\1VgAh/PihaQ5B/WP^RJ51qQPEmHGNmKP]5K@]6^G4sI@U5HPU1]ncpK@U}KG5oJ@g6^nBlIPU5JPMmKmNOQB44Wn^af4hUR\FNfh45WRg4JAtEf~`MQ4Nq\S1jgAtqQPFNeR46WQFVJ4ttWismHGM5^CNoJi]~^G44K@c4HPUsIC]pKPgsJG4~JCQ~J@llJSI4I~AmKmNNfiJ~Wn^RJR5/]7FOeRNk\hg4JAh/VilMQ447WRJ`gAhtWi1Ofh1w\hJOfopU]7ImHGM7]6U~J@c5^m47KPRlHPUsKPMpKC]5J}40K@c7]6Vn^Phn]~MmKmNaeRA6Wi1VJ4tPR\FOQClrWi44JB5q]C5MQC|kWi1RgApP\PFHQCgsWoJNIAtqPPUmHGM~K@U}K@^l^m5lJ~U5HPVmI@IpKP^nJm46^nI4]iI5I6Nh^iUmKmNaU4o4\oFKeQp@W\FNU5^t\S44JApEVP`MQClr\hJVgApERi5HU51r\ShJbotERi<mHGNm]ng0KCM1]m4}KSMsHPU0JiMpKCNnJG45^nllJ@Fl]P^nISUmKmN`eRQ4Wih`J4p@P\FHQ41p\og4JB1UfClMQC=~\hJFgB5q\\|`Q5NkWQFaeQpEUicmHGMs^S^mI~I0KW5l]iNnHPViIPAp]nRi^m47]P]~KSRiJCUsIP]mKmNOQCFqWhJOfB5qQ\FHQ5Q6Wi44JB5UeChMQ41k\i1BgApEVPB`QB1q\S1OfAhtUiomHGNi]iRoI@A~^m5i]nc5HPViKP]p]SQ7^G4}^nhl^iI0JCJo]PMmKmNOUC|p\S1GfAtE^7FaUBQ7Wi44JAtt\\5MQC|rW4FRgB5/]C5`bo5/\RJOfoh@UPQmHGNlIiM}]nloI}4~KCI1HPVlI@Ep]PI6^m5oICNmI~I0JPU|JnQmKmNaUCFkWQFceB1ER\FOJh1r\og4JApE\i1MQC=/Wn^`gAtEQ\|aQ4h~WP^OfB5/]7ImHGM|KPI~]nBlJm41^n]}HPU4]~IpKSM|]W5iJnM5JnU6I~A1JnUmKmNHUBNpWS1VJ51qW\FOUAo7\og4JAt/U\1MQ4A7\i1BgAtqfChOeQtwWoFkfB5tP\4mHGMs]~I5]Po~JG5iJ~^oHPU|JCQpKSU6IG5nIiU0I~]1KSM6J6QmKmNOfo^pWn^Sf55/V\FOboQ5\og4JB1tUi=MQC|pWhFFgB1PW\5OeQA6Wn^SfR5UU\smHGM7^nBn]~]}I}5lKSU0HPVmK@gp]PU5Im5iJPFlK@I5]PNiI~omKmNaQ4As\ihJJottQ\F`QBR<W5g4JB46WPVMQC==Wj1`gB5PQPR`QBB~\i1VJ4htWPAmHGNmKSA~Ing6I}5nI6NnHPVlKCUp]ng1I}5lJ~Q4I6BhI@Jh^CAmKmNNJoA5WQJVIQh@U\F`QC`/W5g4JAt/W\1MQCI|WihVgB46UP^`eQ1kW\1Fbh1EVPQmHGNlJPo4J@A|^W5h]SRlHPUs]~UpKPg0KW47JC^oJ6A7JnI|JPcmKmNNUBR=W5FjJQtEW\FOboQ7WS44JAtqfClMQC|pWShJgAtU]7|Ofh47Wj1afQtPPismHGM7JiM5]6^mJG46^Po0HPU5J@Qp]SM6KG5nIPM~IPA~^P`h]PAmKmNHUB^wWShNg51UQ\FNbiFq\S44JAhtUi|MQC<6WoFFgAp@P\|OeR5/\hJNbok6PicmHGNo]SI|ICMs]W44^nUsHPVmIiQp]SA}^G4|^S^iIPlm]SA}I@UmKmNHQ544WihNIAhPQ\F`QA1qW\44JAhEQP`MQ4Nq\oFNgAtt\PBHQ5B=\\1nJB5PQ\ImHGNlI@NlJ~I6^W40]S]0HPVnJnEp]iM}^W41I@U~KPU5InFlJ~ImKmNNU5R~WoJFIAhtW\FObiJ=\og4JApE]C5MQCJ=WS1BgAhP\i1`Q45<W5JOfB5U]~QmHGM|Ing1]Pc}]W5nICVlHPU}JSAp]PE|]m46]~NoI6A}KS]7K@AmKmNNbhR=\hFJJh1UQ\FaQAA4Wj44JAhUeC=MQ41tWn^JgApPQihNUC=/Wj1JIQhq]~]mHGNo^CI7JCBnJ}5h]nVmHPVh^Pop]PAsIW5m^PU5J~Ni]~^l^CUmKmN`foQ5WS1GeAhtP\FNQBQ|WQg4JAtqVPRMQ4Nt\S1VgB5/PP^HUB5=\oJSfQtq]~UmHGM7]n^mJPU7JG4~I6NiHPVlKSApKCM7IG4|KPg6]PU1]~BhI~AmKmN`boNp\i1JJR1@Q\FNQC|wWi44JAp@P\JMQ4AsWoF`gAhtQi=OUBA|WRJSfAt@\PEmHGM}JCA0JCUs^G5m^i]6HPU7IPIp]nM6KW41]PI6KPo7I~]sIi]mKmNNUCgs\S1RbQhqR\FNUBB/Whg4JAtE^71MQ4NwW\1JgAtq\\|Nbi`<W5FRg4pUVismHGNnJPos^nE0KG5iICUsHPUs]ngp]PM1]}47]n^o^nQ7J~E5^@cmKmNHUCI|\oJVbh5qV\FaQ54s\i44JAhEQP^MQC<s\ihVgB5UR\1NQ4A5WRFVg51PWicmHGNi^Phl]PRoKG4sK@E4HPU4]6Mp]PM~^W5mJCI}Ji]4]nlh^iMmKmN`QAh~\hJVbR5UQ\F`bi<4Whg4JB5URPFMQ45=\i1rgB5/W\JOQC<sWRJGfQp@Ui0mHGMsJ~`mISNlJm5lIno~HPU7J@Ap]iM1I}45^iU|]SBl^Pc}^PImKmNOQA5~WRFOfh1@Q\FOU51kW5g4JAttWilMQ41p\\1jgB1EPi1HQB1tW4FSfB5UR\smHGNiI~o}]SVo^m47^iI0HPU4]iApKP`m]W5lI@c~KSMsInc7^nUmKmNNeQ5/Wj1Wf55/W\FNJo47\Rg4JAhU]~`MQ4A4W\1JgB1PPi5HQ4osWQJKf51tRicmHGM|I@I0^PFi]W47J@o4HPU1JPgp]iUsIG5hJCQ6K@Q0J~^lI@QmKmNNUBR~WihFbot/V\FHQAA5W\44JAh/f7|MQ41t\i1RgB46WPRafhQ|WQFKfh5qPi<mHGM1ISI4IPI5]m5hInBlHPVi^PApK@M~J}4~^iA7]SViIPln^PMmKmNOQA^t\ihNIQhqU\FOJoh=\hg4JAtERPFMQ41pWRJVgAt@U\|aUBNr\S1ceQhP\\0mHGM7JCNiIS^iJG5mInM4HPVh]nEpKP`i^W5iKSNiK@Bn^nJh]ngmKmNNUAo7W4JFJR5tR\FaU544Whg4JAtE]C1MQ4B/W5FVgB5Uf~FObo^p\\1FJ4tE\\ImHGM6]no4]Pc0]m4}K@lmHPVn^CAp]PM1]}44]i]6]PI~]P]}I6UmKmNOJhA5WShJJh5tR\FNQB47Whg4JB5qUPVMQ447WS1RgApPU\1aQAosWoJJg51PRismHGM~InViICUsKW4}]nI0HPU4]~Up]PI4JG4~]6I0ICQs^SQ|Jn]mKmNHQB1q\hFWeQt/\\FOJh^tWRg4JB46Q\5MQClr\RJFgApP\i1`fiI7WRFaeAtUf7ImHGNlI6Qs]6]|Jm45IPo6HPVnJ@gp]SQ5JW5o^iA4]iNiIPc|KSImKmN`eQA4\oJGfoh/R\FOQCFwWj44JAtPUPRMQ4A4\hF`gAtq]~^NUA5<W4FkeQpU]74mHGM0KPU7KCQ}Im47^@M6HPU6JnQpKCVm]W5iK@g4J~A|^n^i]6UmKmNOfh45WShaf4tEV\FaQC`/\og4JAtPW\JMQCI5WSh`gB1tP\1OJh1rWj1VIR5Uf7ImHGM4K@MsJ~I|^G45]nA1HPVnJnUp]i^mJW4}^@^hKPQ6K@M|IPcmKmNNJhA5\\1NJB1@P\FHQ54|\S44JAtUVi5MQCJ=\oFRgAtqf~FOfilqWQFSeB5qP\4mHGM7I6A5]~]}]W5nInA}HPU7^iMp]nFmJW5lI~^iJCI~JCVoK@cmKmNNboB=\\1`IB1qP\F`bo1k\\44JB1@\ilMQCJ~WRJRgAhUUihNUBAsWhFjJh5UUPgmHGM~JnhlJCVo]m44JPg6HPUs^PopK@I1]}41J~I6IiA}J~]~]~UmKmNHUCg5W4FJbB5/U\FOU5A4Wi44JAhqPi|MQC<5WihJgB5U^~RNbo5=W5FOfQh/Qi4mHGM5I~`i^PAs^m5n]nFnHPU5K@opK@M4]W46^Pho]6I5IiM5]n]mKmNOU445\i1rbQt/]7FOJoNw\i44JAo6RPBMQ41wWn^VgB1PPi|OeR1kW5JWfotP\\smHGNnICBi^@`iJm47KPRiHPU7I@ApKPEsIG5iJ6U|IC]|^SU6JiQmKmNOfi=/WShNg51t\\FNbh1p\S44JAhERi=MQC=/\\1jgAt@P\JOU4twWRJRIB1UU\4mHGM5^Pho^PVlIW4~InNmHPVo^@gpK@E6JW47ICU~]nBh]S^iIiQmKmNNeRR~\hFOfB5U\\FNQ5R/Wj44JB5U]~^MQ4B/Wn^NgAtPRil`foQ|Wi1jJ4h@Ri0mHGNhJno5K@RhKW5oJ~g0HPVlJ6IpKPI}^W45JnA5^nU~JCA|JPQmKmNaeRNqWj1nIQt/\\FNfiJ<WS44JAtE^~RMQCJ/WRJFgB5URPRNeQtp\\1jJR46Pi<mHGNi^@M6JCUsJm5lI6JlHPU|]SQpKPcsJ}47J~c}^PIsIiI5J~ImKmNOfoR~WQJGeAhq]7FOJh^pWhg4JAk6P\JMQClq\\1BgB5/Q\|HU5A5Wj1VJ4ttUPEmHGM4ICBnI~FlJW5i^nQ}HPVn^ncp]iI~^G5n]nhiKCU0I@RoJC]mKmNaUCFtWRJKf4h@\\FNfiI6WQg4JB5PWPRMQC|qW5FjgAhEfC=Obi<5WS1RIAhqW\0mHGM~]nRmJiJlIW41InViHPVl]SIpK@hlI}45JPE0K@A7JPE4JPQmKmNNQA^pWj1kfQh@Q\FNUA45W5g4JAt/U\JMQClqWRFNgAtqRP`Ofo5~\S1WeB1EWicmHGM~^CQs]nc|^m5l^CRlHPVo^Pop]SA}IW5hICQ4^S^oICA~JPEmKmNHU5Q4\ihJbB5qW\F`Jo5~Wi44JB1PQ\1MQ45~WS1JgB5/f~RHQA5~W5JNg4t/\PQmHGMs^@`iK@U4IG5nK@]}HPU1^nEpK@o6KG47]nA1JP]sK@Is]P]mKmNHQ4A4WShVbh5/^7FaUB1k\\44JB1PRP`MQ41w\RFNgB1tW\J`Q5^tWS1ceAtEP\4mHGM}JnVhKP]6]}44I6BoHPVmI~UpK@loIm40^@Q5ISI0KPViIPUmKmNaUC|wWihNbot/Q\FaQCJ=\\44JB1EV\5MQ4Nt\RJVgB1EPP`NUB5<W\1JIApPQ\smHGNmI~g4^nM0JG5lK@g5HPVl^PApK@`o^W46]PJmJCJmJPA~^@UmKmNOfi`/\oFVbot/W\FaQ5A4\\44JAtqUPVMQ445\\1BgB5/VPB`fo^q\P^afApEUi0mHGM1I@I5KSU|^m5n^nU6HPU6J@QpKCRmIm45^S^mJ~c4]6BoJnImKmNNU545\P^JJB5qP\FHQ5R/Wog4JAtqRilMQ41p\RFjgB5Uf~VNfh5<W4FNJ51E\PEmHGMsI@A5KS^i^W41^PBoHPU5I@Up]nhhIG45IiJl]~Q4]6ViJC]mKmN`Jo1r\S1Fg4pUW\FaQAo|\i44JAh/VP`MQCI6WP^FgAhqeC1`bo^tWi1VJoht\P]mHGMsI@A1J~M}Jm5h]~VhHPVhJncp]nBo]}4}K@U~ISI6JCI7JCMmKmNHUCJ/WS1weAo6\\FOQ545W\44JB5UR\5MQClr\S1`gAh/Rih`bi==\RFaeB1ERPAmHGNhI6Ms^@E6KW5nJ6M7HPU~J6QpKPFi^G47]nMsJPFmI6]6J~]mKmNaUCFpWhFrJoh/\\FObiJ~Whg4JAo6U\5MQ4B~WoFVgB5/W\5NfoQsW4JWfR1@UPQmHGM0^PM~InNh^m44]SQ6HPVo]6Up]n^o^W40Ji]sJCBoKCU}^SQmKmNObi<7Wj1Kf51@Q\FNeR5~\og4JB5PUi=MQ4NkWoFngAtPRPRNUC=~WhJJJQk6PicmbU99";
            string json = Archiver.Decode(Key);

            Dictionary<string, string> res = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            List<UserDB> list = new List<UserDB>();
            foreach (var item in res)
            {
                UserDB user = new UserDB();
                user.Key = item.Key;
                user.Password = item.Value;
                user.Roles = Roles.User;
                user.Hash = Archiver.Encode(item.Key + item.Value);

                list.Add(user);
            }

            UserDB userAdmin = new UserDB();
            userAdmin.Key = "uvridheofj07y8dop812tjlwdf9rxhb0";
            userAdmin.Password = "nbezoy12jl";
            userAdmin.Roles = Roles.Admin;
            userAdmin.Hash = "ghfudjkdkdl33333dddddd";


          

            dbContext.Users.Add(userAdmin);
            dbContext.Users.AddRange(list);

            dbContext.SaveChanges();
        }
    }
}
