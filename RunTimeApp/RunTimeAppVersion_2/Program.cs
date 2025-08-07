using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RunTimeAppVersion_2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Init();

            double stepsLength = GetStepsLength();   //adım uzunluğu
            int stepsCount = GetStepsCount();        //dakikada atılan adım sayısı
            int runTimeHour = GetRunTimeHour();      //koşu saati
            int runTimeMinute = GetRunTimeMinute(runTimeHour); //koşu dakikası

            Console.Clear();

            int totalRunTime = GetDistance(stepsLength, stepsCount, runTimeHour, runTimeMinute); //Koşunun toplam mesafe hesabı ve bir değişkene atanması. 

            DateTime startTime = DateTime.Now; //Koşuya başlandığındaki gerçek zaman.
           
            double firstDistance = 0; // İlk mesafeyi tutacak
            int remainingTime = totalRunTime; // Kalan süreyi tutacak 

            while (remainingTime > 0)   //kalan süre 0 olana kadar adım güncelleme seçeneği karşımıza çıkıyor.
            {
                bool devam = Exit();
                if (!devam)
                {
                    Console.WriteLine("Uygulama sonlandırılıyor...");
                    break;
                }

                GetStepsCountAgain(ref stepsLength, ref stepsCount, ref remainingTime, ref firstDistance, startTime);

                if (remainingTime <= 0)
                {
                    Console.WriteLine("Harika! Bugünkü antrenmanınızı tamamladınız!");
                    break;
                }

            }

        }

        static bool Exit()
        {
            Console.WriteLine("Çıkış yapmak için 'C' tuşuna, devam etmek için 'Enter' tuşuna basın.");
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            if (keyInfo.Key == ConsoleKey.C)
            {
                return false;
            }
            else if (keyInfo.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                return true;
            }
            else
            {
                return Exit();
            }

        }

        static void GetStepsCountAgain(ref double stepsLength, ref int stepsCount, ref int remainingTime, ref double firstDistance, DateTime startTime)
        {

            Console.WriteLine("          ----Hatırlatıcı!----          ");
            Console.WriteLine("Adım sayınızı güncellemek isterseniz 'N' tuşuna tıklayabilirsiniz. \n Devam etmek için 'D' tuşuna basın.");

            char keyChar = Console.ReadKey(true).KeyChar;

            DateTime updateTime = DateTime.Now;                       // yeniden adım girmek istediğindeki gerçek dakika.
            // TimeSpan timeDifference = updateTime - startTime;      // geçen sürenin dakika karşılığı gerçek zamanlı.
            //int timeSpan = (int)timeDifference.TotalMinutes;
            int timeSpan = 30;                                        // burada elle değer verdim kontrol amaçlı. Gerçek uygulama deneyiminde üstteki 2 satır çalıştırılacak.

            remainingTime -= timeSpan; // kalan süre= kalan süre - geçen süre.
            if (keyChar == 'N' || keyChar == 'n')
            {
                if (remainingTime <= 0)
                {
                    return;
                }

                firstDistance = stepsLength / 100 * stepsCount * timeSpan; //Koşunun ilk kısmında ne kadar mesafe kat edildi?

                stepsCount = GetStepsCount();  //burda tekrar 1 dakikada atılan adım sayısı alınıyor. Kalan zaman üzerinden kalan mesafe hesaplanacak.
                Console.WriteLine($"Antrenmanınızda {firstDistance} metreyi tamamladınız. Kalan {remainingTime} dakika için mesafe yeniden hesaplanıyor.");
                Thread.Sleep(2000);

                double distancePerMinute = stepsLength / 100 * stepsCount;
                double totalDistance = distancePerMinute * remainingTime + firstDistance; // toplam mesafe = 1 dkda gidilen mesafe * kalan süre + daha önce koşulan mesafe
                Console.WriteLine($"Koşulacak toplam mesafe = {(totalDistance).ToString("N0")} metre");
                Console.WriteLine();
            }
            else if (keyChar == 'D' || keyChar == 'd')
            {
                if (remainingTime <= 0)
                {
                    return;
                }
                remainingTime -= timeSpan;
                firstDistance = stepsLength / 100 * stepsCount * timeSpan;
                Console.WriteLine($"Antrenmanınızda {firstDistance} metreyi tamamladınız. Kalan süre {remainingTime} dk.");
            }

        }

        static void Init()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            Console.Write("            ----- Hoşgeldiniz");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" \u2665 -----            ");
            Console.ResetColor();
            Console.WriteLine();

            Console.WriteLine("Lütfen aşağıdaki seçenekleri size en uygun şekilde doldurunuz.");
            Console.WriteLine("         -------            -------            -------         ");
        }

        static double GetStepsLength()
        {
            double stepsLength;

            Console.Write("Ortalama adım uzunluğunuz(cm): ");
            bool input = double.TryParse(Console.ReadLine(), out stepsLength);

            while (true)
            {
                if (stepsLength > 65 && stepsLength < 150)
                {
                    return stepsLength;
                }
                else
                {
                    Console.Write("Bir insanın ortalama adım uzunluğu 65-150 cm aralığındadır.\n Lütfen bu aralıkta bir değer giriniz: ");
                    input = double.TryParse(Console.ReadLine(), out stepsLength);
                }

            }
        }

        static int GetStepsCount()
        {
            int stepsCount;

            Console.Write("1 dakikada atılan ortalama adım sayısı: ");
            bool input = int.TryParse(Console.ReadLine(), out stepsCount);
            while (true)
            {
                if (stepsCount > 20 && stepsCount < 150)
                {
                    return stepsCount;
                }
                else
                {
                    Console.Write("Lütfen 20-150 aralığında bir değer giriniz: ");
                    input = int.TryParse(Console.ReadLine(), out stepsCount);
                }

            }
        }

        static int GetRunTimeHour()
        {
            int runTimeHour;

            Console.WriteLine("Koşu süresi:");
            Console.Write("Saat: ");
            bool input = int.TryParse(Console.ReadLine(), out runTimeHour);
            while (true)
            {
                if (runTimeHour >= 0 && runTimeHour < 24)
                {
                    return runTimeHour;
                }
                else
                {
                    Console.WriteLine("Lütfen saat için 0-24 aralığında bir değer giriniz.");
                    Console.Write("Saat: ");
                    input = int.TryParse(Console.ReadLine(), out runTimeHour);
                }

            }

        }

        static int GetRunTimeMinute(int runTimeHour)
        {
            int runTimeMinute;

            Console.Write("Dakika:");
            bool input = int.TryParse(Console.ReadLine(), out runTimeMinute);
            while (true)
            {

                if (runTimeMinute >= 0 && runTimeMinute < 60)
                {
                    return runTimeMinute;
                }
                else if (runTimeMinute == 60)
                {
                    return runTimeHour += runTimeMinute;
                }
                else
                {
                    Console.WriteLine("Lütfen dakika için 0-60 aralığında bir değer giriniz.");
                    Console.Write("Dakika:");
                    input = int.TryParse(Console.ReadLine(), out runTimeMinute);
                }

            }
        }

        static int GetDistance(double stepsLength, int stepsCount, int runTimeHour, int runTimeMinute)
        {
            stepsLength /= 100;  //metre cinsinden                                        
            double distancePerMinute = stepsLength * stepsCount;            //1 dakikada gidilen mesafe
            int totalRunTime = (runTimeHour * 60) + runTimeMinute;           //Koşu yapılacak toplam süre.
            double totalDistance = distancePerMinute * totalRunTime;      //Toplam mesafe = 1 dakikada gidilen mesafe * toplam dakika.

            string min = runTimeMinute < 10 ? "0" + runTimeMinute.ToString() : runTimeMinute.ToString();
            string hour = runTimeHour < 10 ? "0" + runTimeHour.ToString() : runTimeHour.ToString();

            Console.WriteLine($"Toplam antrenman süresi : {hour} : {min}");
            Console.WriteLine($"Koşulacak toplam mesafe = {(totalDistance).ToString("N0")} metre");
            Console.WriteLine();
            return totalRunTime;

        }

    }
}
