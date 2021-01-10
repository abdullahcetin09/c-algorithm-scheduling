using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace çizelgeleme
{
    public partial class Form1 : Form
    {
        public int i, k;
        public int issayisi, issuresi, makinesayisi,cmax,topbos;
        public int[] m_siralama;
        public int[] n_siralama;
        public int[] m2_siralama;
        public int[] n2_siralama;
        public int[] makboskalma;
        public int[] toplamissure;
        public int[] alternatif_sıralama;
        public int[] aj;
        public int[,] isler;
        public int[,] gecici_alt;
        public int[,] tamamlanmaZamani;
        public int[,] siralanmisisler;
        public int[,] alt_isler;
        public int[,] csd;
        public Dictionary<string, string> siralama =
                     new Dictionary<string, string>();
        public string srlm;
        public Boolean degis3 = false;

        public Form1()
        {
            InitializeComponent();
            listBox1.Items.Clear();
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            issayisi = Convert.ToInt32(textBox1.Text);
            makinesayisi = Convert.ToInt32(textBox3.Text);
            i = 0;
            k = 0;
            isler = new int[makinesayisi, issayisi];
            label2.Text = Convert.ToString(k + 1) + ".nci makine için" + Convert.ToString(i + 1) + ".iş süresi";
            label2.Visible = true;
            textBox2.Visible = true;
            button2.Visible = true;
            label3.Text = "";
        }
        public void makine_sure()
        {//makine 1 den sonraki makinelerdeki işlem sürelerinin hesaplar
            int sayac = 0;

            tamamlanmaZamani = new int[makinesayisi, issayisi];
            for (i = 0; i < makinesayisi; i++) {
                for (k = 0; k < issayisi; k++)
                {
                    if (k == 0 && i != 0)
                    {
                        sayac = sayac + siralanmisisler[i, k];
                        tamamlanmaZamani[i, k] = sayac;
                        siralama.Add(Convert.ToString(i) + "-" + Convert.ToString(k), Convert.ToString(tamamlanmaZamani[i-1,k]) + "-" + Convert.ToString(sayac));
                    }
                    if (k == 0 && i == 0)
                    {
                        sayac = sayac + siralanmisisler[i, k];
                        tamamlanmaZamani[i, k] = sayac;
                        siralama.Add(Convert.ToString(i) + "-" + Convert.ToString(k), Convert.ToString(0) + "-" + Convert.ToString(sayac));
                    }
                    if (i == 0 && k != 0)
                    {
                        tamamlanmaZamani[i, k] = tamamlanmaZamani[i, k - 1] + siralanmisisler[i, k];
                        siralama.Add(Convert.ToString(i) + "-" + Convert.ToString(k), Convert.ToString(tamamlanmaZamani[i, k - 1]) + "-" + Convert.ToString(tamamlanmaZamani[i, k]));
                    }
                    if (i != 0 && k != 0)
                    {
                        if (tamamlanmaZamani[i, k - 1] >= tamamlanmaZamani[i - 1, k]) { tamamlanmaZamani[i, k] = tamamlanmaZamani[i, k - 1] + siralanmisisler[i, k];
                            siralama.Add(Convert.ToString(i) + "-" + Convert.ToString(k), Convert.ToString(tamamlanmaZamani[i, k - 1]) + "-" + Convert.ToString(tamamlanmaZamani[i, k]));
                        }
                        if (tamamlanmaZamani[i, k - 1] < tamamlanmaZamani[i - 1, k]) { tamamlanmaZamani[i, k] = tamamlanmaZamani[i - 1, k] + siralanmisisler[i, k];
                            siralama.Add(Convert.ToString(i) + "-" + Convert.ToString(k), Convert.ToString(tamamlanmaZamani[i-1, k]) + "-" + Convert.ToString(tamamlanmaZamani[i, k]));
                        }
                        
                    }

                    //MessageBox.Show(Convert.ToString(tamamlanmaZamani[i, k]));

                }
            }
        }
        public void siralimatris()
        {
            siralanmisisler = new int[makinesayisi, issayisi];
            for (i = 0; i < makinesayisi; i++)
            {
                for (k = 0; k < issayisi; k++)
                {siralanmisisler[i, k] = isler[i, n_siralama[k]];
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            srlm = "";
            if (makinesayisi == 3)
            {
                
                matrisyenileme();
                degis3 = false;
                              
            }
            else { degis3 = true; }
                dizme();
                siralimatris();
                makine_sure();
                issureleritop();
                bostakalma();
                cmax = tamamlanmaZamani[makinesayisi - 1, issayisi - 1];

                listBox1.Items.Add("cmax :" + Convert.ToString(cmax));
                listBox1.Items.Add("toplam bosta kalma :" + Convert.ToString(topbos));
                for (i = 0; i < n_siralama.Length; i++) { srlm = srlm + Convert.ToString(n_siralama[i] + 1) + " - "; }
                listBox1.Items.Add(srlm);

                for (i = 0; i < makinesayisi; i++) { listBox1.Items.Add("makine " + Convert.ToString(i + 1) + " bosta kalma suresi :" + Convert.ToString(makboskalma[i])); }
                for (i = 0; i < makinesayisi; i++)
                {
                    for (k = 0; k < issayisi; k++)
                    {
                        listBox1.Items.Add(Convert.ToString(i + 1) + "makine" + Convert.ToString(k + 1) + ".iş başlama-bitis zamanı :" + siralama[Convert.ToString(i) + "-" + Convert.ToString(k)]);
                    }
                }

            yenileme();
        }
        public void bostakalma() {
            makboskalma = new int[makinesayisi];
            topbos = 0;
            for (int k = 0; k < makinesayisi; k++)
            {   
                makboskalma[k] = tamamlanmaZamani[k, issayisi-1] - toplamissure[k];
                topbos = topbos + makboskalma[k];
               // MessageBox.Show(Convert.ToString(makboskalma[k]));
            }
        }
        public void issureleritop()
        {
            toplamissure = new int[makinesayisi];
            for (int k = 0; k < makinesayisi; k++)
            {
               int sayac = 0;
                for (int i = 0; i < issayisi; i++)
                {
                    sayac = sayac+isler[k, i];
                }
                toplamissure[k] = sayac;
                //MessageBox.Show(Convert.ToString(toplamissure[k]));
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            srlm = "";
            ajsira();
            siralimatris();
            makine_sure();
            issureleritop();
            bostakalma();
            cmax = tamamlanmaZamani[makinesayisi - 1, issayisi - 1];

            listBox1.Items.Add("cmax :" + Convert.ToString(cmax));
            listBox1.Items.Add("toplam bosta kalma :" + Convert.ToString(topbos));
            for (i = 0; i < n_siralama.Length; i++) { srlm = srlm + Convert.ToString(n_siralama[i] + 1) + " - "; }
            listBox1.Items.Add(srlm);
            for (i = 0; i < makinesayisi; i++) { listBox1.Items.Add("makine " + Convert.ToString(i + 1) + " bosta kalma suresi :" + Convert.ToString(makboskalma[i])); }
            for (i = 0; i < makinesayisi; i++)
            {
                for (k = 0; k < issayisi; k++)
                {
                    listBox1.Items.Add(Convert.ToString(i + 1) + "makine" + Convert.ToString(k + 1) + ".iş başlama-bitis zamanı :" + siralama[Convert.ToString(i) + "-" + Convert.ToString(k)]);
                }
            }
            yenileme();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_Click(object sender, EventArgs e)
        {
            textBox3.Clear();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            
            degis3 = true;
            cdsmatris();
            /* for (i = 0; i < makinesayisi - 1; i++)
             {
                 for (k = 0; k < issayisi+1; k++)
                 {
                     MessageBox.Show(Convert.ToString(csd[i, k])+"csd matris");
                 }
             }*/
            int[] cmaxdizi = new int[makinesayisi - 1];
            for (k = 0; k < makinesayisi-1; k++) {cmaxdizi[k] = csd[k, issayisi];}
            //min cmax hesplama
            int min=cmaxdizi[0];
            int min_indeks = 0;
            for (k = 0; k < makinesayisi - 1; k++) {
                if (min > cmaxdizi[k])
                {
                    min = cmaxdizi[k];
                    min_indeks = k;
                }
            }
            siralama =
                    new Dictionary<string, string>();
            srlm = "";
            for (k = 0; k <issayisi; k++) { n_siralama[k] = csd[min_indeks, k]; }
            siralimatris();
            makine_sure();
            issureleritop();
            bostakalma();
            cmax = tamamlanmaZamani[makinesayisi - 1, issayisi - 1];
            listBox1.Items.Add("cmax :" + Convert.ToString(cmax));
            listBox1.Items.Add("toplam bosta kalma :" + Convert.ToString(topbos));
            for (i = 0; i < n_siralama.Length; i++) { srlm = srlm + Convert.ToString(n_siralama[i] + 1) + " - "; }
            listBox1.Items.Add(srlm);

            for (i = 0; i < makinesayisi; i++) { listBox1.Items.Add("makine " + Convert.ToString(i + 1) + " bosta kalma suresi :" + Convert.ToString(makboskalma[i])); }
            for (i = 0; i < makinesayisi; i++)
            {
                for (k = 0; k < issayisi; k++)
                {
                    listBox1.Items.Add(Convert.ToString(i + 1) + "makine" + Convert.ToString(k + 1) + ".iş başlama-bitis zamanı :" + siralama[Convert.ToString(i) + "-" + Convert.ToString(k)]);
                }
            }
            yenileme();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            issuresi = Convert.ToInt32(textBox2.Text);

            isler[k, i] = issuresi;
           // MessageBox.Show(Convert.ToString(isler[k, i]));
            label3.Visible = true;
            label3.Text = "is suresi basari ile kaydedildi....";
            textBox2.Clear();

            i = i + 1;
            
            
            if (i == issayisi) {
                k = k + 1;
                i = 0;
            }



            if (k == makinesayisi) { 
                if (i == 0 )
                {
                label2.Visible = false;
                textBox2.Visible = false;
                button2.Visible = false;
                }
            }
            
            label2.Text = Convert.ToString(k + 1) + "makine için " + Convert.ToString(i + 1) + ".iş süresi";

        }
        public void dizme()
        {   
            
            int min,gecicisayi;
            
            m_siralama =new int[issayisi];
            n_siralama = new int[issayisi];
            m2_siralama = new int[issayisi];
            n2_siralama = new int[issayisi*2];
            int [] tum = new int[issayisi*2];
           if (degis3 == false) {
                makinesayisi = 2;
                isler = new int[makinesayisi, issayisi];
                isler = alt_isler;
                degis3 = true;
            }

            

            int sayac = 0;
           
            for (i = 0; i < makinesayisi; i++)
            {
                for (k = 0; k < issayisi ; k++)
                { tum[sayac] = isler[i, k];
                    n2_siralama[sayac] = sayac;sayac =sayac+1;
                   // MessageBox.Show(Convert.ToString(tum[sayac]) + "sıra degeri" + Convert.ToString(n2_siralama[sayac]));
                }
            }
                 
            for (i = 0; i < (issayisi * 2); i++)
            {
                for (k = 0; k < (issayisi * 2)-1; k++)
                {
                    if (tum[k] > tum[k + 1])
                    {
                        gecicisayi = tum[k];
                        tum[k] = tum[k + 1];
                        tum[k + 1] = gecicisayi;
                        min = n2_siralama[k];
                        n2_siralama[k] = n2_siralama[k + 1];
                        n2_siralama[k + 1] = min;
                    }
                }
                
            }
            //küçükten büyüğe sıralama kontrolü
            //for (i = 0; i < n2_siralama.Length; i++) { MessageBox.Show(Convert.ToString(n2_siralama[i])); }
            int sayac1=1;
            sayac=0;
            Boolean degis = false;
            Boolean degis1 = false;
            Boolean degis2 = false;
            alternatif_sıralama = new int[issayisi];
            int atlama = 0;
          
            for (i = 0;i< issayisi+ atlama; i++) {
                for (k = 0; k < issayisi; k++)
                {
                    if (n2_siralama[i] < issayisi)
                    {
                        if (n_siralama[k] == n2_siralama[i] || alternatif_sıralama [k]== n2_siralama[i]) { degis1 = true; }
                    }
                    else { if (n_siralama[k] == n2_siralama[i]% issayisi || alternatif_sıralama[k] == n2_siralama[i]% issayisi) { degis1 = true; } }
                   
                }
                if (degis2 == false) { if (n2_siralama[i] == 0 || n2_siralama[i]%issayisi==0) { degis1 = false; degis2 = true; } }
               
                
                if (degis1 == false){
                    if (tum[i] == tum[i + 1])
                    {
                        if (n2_siralama[i] < issayisi && n2_siralama[i + 1] >= issayisi)
                        {
                            if (n2_siralama[i] == n2_siralama[i + 1] % issayisi)
                            {
                                n_siralama[sayac] = n2_siralama[i];
                                if (degis == true)
                                { alternatif_sıralama[sayac] = n2_siralama[i]; }
                                sayac = sayac + 1;
                                //MessageBox.Show(Convert.ToString(n2_siralama[i]) + "durum özel" + Convert.ToString(i));
                            }
                            else
                            {

                                n_siralama[sayac] = n2_siralama[i];
                                n_siralama[issayisi - (sayac1)] = n2_siralama[i + 1] % issayisi;
                                if (degis == true)
                                {
                                    alternatif_sıralama[sayac] = n2_siralama[i];
                                    alternatif_sıralama[issayisi - (sayac1)] = n2_siralama[i + 1] % issayisi;
                                }
                               // MessageBox.Show(Convert.ToString(n2_siralama[i]) + "durum 1" + Convert.ToString(i));
                                sayac1 = sayac1 + 1;
                                sayac = sayac + 1;
                                i = i + 1;
                            }
                        }
                        else
                        {
                            if (n2_siralama[i] < issayisi)
                            {
                                if (degis == false) { alternatif_sıralama = n_siralama; degis = true; }
                                n_siralama[sayac] = n2_siralama[i];
                                alternatif_sıralama[sayac] = n2_siralama[i + 1];
                                n_siralama[sayac + 1] = n2_siralama[i + 1];
                                alternatif_sıralama[sayac + 1] = n2_siralama[i];
                                //MessageBox.Show(Convert.ToString(n2_siralama[i]) + "durum 2" + Convert.ToString(i));
                                sayac = sayac + 2;
                                i = i + 1;
                               
                            }
                            else
                            {
                                if (degis == false) { alternatif_sıralama = n_siralama; degis = true; }
                                n_siralama[issayisi - sayac1] = n2_siralama[i] % issayisi;
                                n_siralama[issayisi - (sayac1 + 1)] = n2_siralama[i + 1] % issayisi;
                                alternatif_sıralama[issayisi - sayac1] = n2_siralama[i + 1] % issayisi;
                                alternatif_sıralama[issayisi - sayac1 - 1] = n2_siralama[i] % issayisi;
                                //MessageBox.Show(Convert.ToString(n2_siralama[i]) + "durum 3" + Convert.ToString(i));
                                sayac1 = sayac1 + 2;
                                i = i + 1;
                               
                            }

                        }



                    }
                    else
                    {
                        if (n2_siralama[i] < issayisi)
                        {

                            n_siralama[sayac] = n2_siralama[i];
                            if (degis == true)
                            { alternatif_sıralama[sayac] = n2_siralama[i]; }
                           sayac = sayac + 1;
                            //MessageBox.Show(Convert.ToString(n2_siralama[i]) + "durum4" + Convert.ToString(i));
                        }
                        else
                        {
                            n_siralama[issayisi - sayac1] = n2_siralama[i] % issayisi;
                            if (degis == true)
                            { alternatif_sıralama[issayisi - sayac1] = n2_siralama[i] % issayisi; }
                            sayac1 = sayac1 + 1;
                            //MessageBox.Show(Convert.ToString(n2_siralama[i])+"durum5 " + Convert.ToString(i));
                        }

                    }



                }
                else { atlama++; degis1 = false;
                    //MessageBox.Show(Convert.ToString(n2_siralama[i]) + "durum 6 "+ Convert.ToString(i)); 
                 
                }
            }
            
            
        }
        
        public void ajsira()
        {
            aj = new int[issayisi];
            int  [] ajkat = new int[makinesayisi];
            for (i = 0; i < makinesayisi; i++) { ajkat[i] = makinesayisi - ((2 * (i+1)) - 1); }
            
            for (i = 0; i < issayisi; i++)
            {
                int sayac = 0;
                for (k = 0; k<makinesayisi ; k++)
                {
                 sayac =sayac + ajkat[k] * isler[k, i];
                    
                }
                aj[i] = -1 * sayac;

            }
            for (i = 0; i < issayisi; i++) { MessageBox.Show(Convert.ToString(aj[i])); }
            int min ;
            int gecici;
            int[] kt = new int[issayisi];
            for (i = 0; i < issayisi; i++) { kt[i] = i; }
            for (k = 0; k < issayisi; k++)
            {
                for (i = 0; i < issayisi; i++)
                {
                    if (aj[i] < aj[k])
                    {
                        gecici = aj[i];
                        aj[i] = aj[k];
                        aj[k] = gecici;
                        min = kt[i];
                        kt[i] = kt[k];
                        kt[k] = min;
                       
                    }

                }
            }


            n_siralama = kt;
            
        }
        public void matrisyenileme() {
            alt_isler = new int[2, issayisi];
            int t = 0;
            for (i = 0; i <2; i++)
            {
                
                for (k = 0; k < issayisi; k++){

                     t=isler[i, k] + isler[i+1, k];
                     alt_isler[i, k] = t;
                    //MessageBox.Show(Convert.ToString(alt_isler[i, k]));
                }
            }

        }
        public void cdsmatris()
        {
            int[,] gecici_isler = new int[makinesayisi, issayisi];
            int[,] gecici_alt= new int[2, issayisi];
            csd = new int[makinesayisi-1, issayisi + 1];
            gecici_isler = isler;
            int gecicimakine = makinesayisi;
            int sayac = 0;
            int sayac1 = 0;
            
            for (int t = 0; t < makinesayisi - 1; t++)
            {
                
                for (i = 0; i < issayisi; i++)
                {
                    for (k = 0; k < t + 1; k++)
                    {
                        sayac = sayac + gecici_isler[k, i];
                        sayac1 = sayac1 + gecici_isler[makinesayisi - 1 - k, i];
                    }
                    gecici_alt[0, i] = sayac;
                    gecici_alt[1, i] = sayac1;
                    sayac = 0;
                    sayac1 = 0;
                }
                isler = new int[2, issayisi];
                isler = gecici_alt;
                //for (i = 0; i < issayisi; i++) { MessageBox.Show(Convert.ToString(gecici_alt[0, i]) + Convert.ToString(gecici_alt[1, i]) + "geçici'" +Convert.ToString(isler[0, i]) + Convert.ToString(isler[1, i]) + "isler'"); }
                //alt islerin deger kontrolü
                //for (i = 0; i < issayisi; i++) { MessageBox.Show(Convert.ToString(isler[0, i]) + "m1'" + Convert.ToString(isler[1, i]) + "M2'"); }
                makinesayisi = 2;
                dizme();
                //for (i = 0; i < issayisi; i++) { MessageBox.Show(Convert.ToString(n_siralama[i])+ "dizme kontrol"); }//dizme kontrol
                
                //siralimatris();
                //makine_sure();
               // issureleritop();
              //  bostakalma();
                
                for (i = 0; i < issayisi; i++) { csd[t, i] = n_siralama[i]; }
               // csd[t, issayisi] = cmax;

                isler = new int[makinesayisi, issayisi];
                isler = gecici_isler;
                makinesayisi = gecicimakine;
                siralama =
                     new Dictionary<string, string>();
                srlm = "";
               // dizme();
               siralimatris();
                makine_sure();
                issureleritop();
                bostakalma();
                cmax = tamamlanmaZamani[makinesayisi - 1, issayisi - 1];
                csd[t, issayisi] = cmax;
            }
        }
        
        public void neh_ilkiki()
        {
            int[] ilksira = new int[issayisi];
            int[] gecici_cmax ;
            int sayac,gecici_issayisi;
            int[,] gecici_isler = new int[makinesayisi, issayisi];
            gecici_isler = isler;
            gecici_issayisi = issayisi;
            for (i = 0; i < issayisi; i++)
            {
                sayac = 0;
                for (k = 0; k <makinesayisi; k++)
                {
                    sayac = sayac + isler[k, i];
                }
                ilksira[i] = sayac;
            }
            int gecicisayi,min;
            int[] gecici_sira = new int[issayisi];
            for (i = 0; i < issayisi; i++) { gecici_sira[i] = i; }
            for (i = 0; i < issayisi; i++)
            {
                for (k = 0; k < issayisi - 1; k++)
                {
                    if (ilksira[i]>ilksira[k]) {
                        gecicisayi = ilksira[k];
                        ilksira[k] = ilksira[k + 1];
                        ilksira[k + 1] = gecicisayi;
                        min = gecici_sira[k];
                        gecici_sira[k] = gecici_sira[k + 1];
                        gecici_sira[k + 1] = min;
                    }
                }
            }
            MessageBox.Show(Convert.ToString(gecici_sira[0]) + "------" + Convert.ToString(gecici_sira[1]));
            int sayac4 = 2;

            int[,] gecici_is = new int[makinesayisi,sayac4];
            
            for (i = 0; i < makinesayisi; i++)
            {
                for ( k = 0; k < sayac4; k++)
                {
                    gecici_is[i, k] = isler[i, gecici_sira[k]];
                }
            }
            isler = new int[makinesayisi, sayac4];
            isler = gecici_is;
            issayisi = sayac4;
            gecici_cmax = new int[sayac4];
            siralama =
                    new Dictionary<string, string>();
            srlm = "";
            // dizme();
            siralimatris();
            makine_sure();
            issureleritop();
            bostakalma();
            cmax = tamamlanmaZamani[makinesayisi - 1, issayisi - 1];
            





        }
        public void yenileme()
        {
            siralama =
                    new Dictionary<string, string>();
            srlm = "";
        }



    }
}
