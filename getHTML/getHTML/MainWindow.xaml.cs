using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
//using HtmlAgilityPack;
using System.Windows.Forms;
using System.Net;

namespace getHTML
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        
        int step = -1;
        string timustr = "";
        int timer_count = 0;
        Timer timer = new Timer();
        List<int> pages = new List<int>();
        List<string> flag = new List<string>();
        int lastpages;
        int flagstep = 0;
        private void getbtn_Click(object sender, RoutedEventArgs e)
        {
            getbtn.IsEnabled = false;
            //step += 100;
            parastack.Opacity = 0;
            for (int i = 2; i <= int.Parse(counttxt.Text); i++)
            {
                pages.Add(i);
            }
            lastpages = int.Parse(counttxt.Text);
            wblist.Navigate(new Uri(linktxt.Text + "1"));
            //wb.Navigate(new Uri("http://www.jiakaobaodian.com/tiku/shiti/" + step + ".html"));


        }

        private void clickwb()
        {
            wb.Navigate(new Uri("http://www.jiakaobaodian.com/tiku/shiti/" + flag[flagstep] + ".html"));
        }

        


        private void wb_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //wb.DocumentCompleted-=new WebBrowserDocumentCompletedEventHandler(wb_DocumentCompleted);
            int isrightcount = 0;
            if (e.Url.ToString() != wb.Url.ToString())
                return;
            //timustr = timu.Text;
            timu.Text = "";
            op1.Text = "";
            op2.Text = "";
            op3.Text = "";
            op4.Text = "";

            //randtxt.Text = wb.Url.ToString();
            randtxt.Text = flag.Last();
            string rand = "";
            img1.Opacity = 0;
            img2.Opacity = 0;
            img3.Opacity = 0;
            img4.Opacity = 0;
            int tixing = 0;
            bool tp=false;
            int pddc=0;
                        int isright1=0;
            int isright2=0;
            int isright3=0;
            int isright4=0;
            int imgid = 0;
            string imgsrc="";
            //timu.Text = "";
            //System.Windows.Forms.Application.DoEvents();
            HtmlElementCollection h1 = wb.Document.GetElementsByTagName("h1");
            //System.Windows.MessageBox.Show(wb.Document.ToString());
            foreach (HtmlElement elem in h1)
            {
                if (elem.GetAttribute("className").Equals("shiti-content")) 
                {
                    //System.Windows.MessageBox.Show(elem.OuterText);
                    HtmlElementCollection b = elem.Children as HtmlElementCollection;


                    if (elem.OuterText.ToString().Substring(0,2)=="难 ")
                    {
                        timu.Text = elem.OuterText.ToString().Substring(2, elem.OuterText.ToString().Length - 2);
                    }
                    else
                    {
                        timu.Text = elem.OuterText;
                    }
                    break;
                }
            }

            HtmlElementCollection answers = wb.Document.GetElementsByTagName("p");
            foreach (HtmlElement answer in answers)
            {
                if (answer.GetAttribute("data-answer").Equals("128"))
                {
                    tixing++;
                    //System.Windows.MessageBox.Show(answer.Children[1].OuterText);
                    op1.Text = answer.Children[1].OuterText.ToString().Substring(3, answer.Children[1].OuterText.ToString().Length - 3);
                    if (answer.GetAttribute("className").Equals("dui "))
                    {
                        img1.Opacity = 1;
                        isright1=1;
                        isrightcount++;
                    }
                }
                if (answer.GetAttribute("data-answer").Equals("64"))
                {
                    tixing++;
                    //System.Windows.MessageBox.Show(answer.Children[1].OuterText);
                    op2.Text = answer.Children[1].OuterText.ToString().Substring(3, answer.Children[1].OuterText.ToString().Length - 3);
                    if (answer.GetAttribute("className").Equals("dui "))
                    {
                        img2.Opacity = 1;
                        isright2=1;
                        isrightcount++;
                    }
                }
                if (answer.GetAttribute("data-answer").Equals("32"))
                {
                    tixing++;
                    //System.Windows.MessageBox.Show(answer.Children[1].OuterText);
                    op3.Text = answer.Children[1].OuterText.ToString().Substring(3, answer.Children[1].OuterText.ToString().Length - 3);
                    if (answer.GetAttribute("className").Equals("dui "))
                    {
                        img3.Opacity = 1;
                        pddc=32;
                        isright3=1;
                        isrightcount++;
                    }
                }
                if (answer.GetAttribute("data-answer").Equals("16"))
                {
                    tixing++;
                    //System.Windows.MessageBox.Show(answer.Children[1].OuterText);
                    op4.Text = answer.Children[1].OuterText.ToString().Substring(3, answer.Children[1].OuterText.ToString().Length - 3);
                    if (answer.GetAttribute("className").Equals("dui "))
                    {
                        img4.Opacity = 1;
                        pddc=16;
                        isright4=1;
                        isrightcount++;
                    }
                }
            }
            steptxt.Text = flag[flagstep];
            if (tixing > 2)
            {
                tixingtxt.Text = "选择";
            }
            else
            {
                tixingtxt.Text = "判断";
            }


            HtmlElementCollection imgs = wb.Document.GetElementsByTagName("div");
            foreach (HtmlElement img in imgs)
            {
                if (img.GetAttribute("data-item").Equals("media-container"))
                {
                    //System.Windows.MessageBox.Show(img.Children[0].GetAttribute("src"));
                    tp=true;
                    //downfile(img.Children[0].GetAttribute("src"));
                    imgsrc = img.Children[0].GetAttribute("src");
                }
            }



            /////////////////////写入数据库//////////////////////////
            getHTML.questionDataSet questionDataSet = ((getHTML.questionDataSet)(this.FindResource("questionDataSet")));
            // 将数据加载到表 questions 中。可以根据需要修改此代码。
            getHTML.questionDataSetTableAdapters.questionsTableAdapter questionDataSetquestionsTableAdapter = new getHTML.questionDataSetTableAdapters.questionsTableAdapter();
            questionDataSetquestionsTableAdapter.Fill(questionDataSet.questions);
            
            getHTML.questionDataSetTableAdapters.answersTableAdapter questionDataSetanswersTableAdapter = new getHTML.questionDataSetTableAdapters.answersTableAdapter();
            questionDataSetanswersTableAdapter.Fill(questionDataSet.answers);


            int pd=0;
            if(tixing>2)
            {
                pd=1;
            }

            string questiontype;
            if(pd==0)
            {
                questiontype="PD";
            }
            else
            {
                questiontype="XZ";
            }

            if(tp)
            {
                questiontype+=":TP";
            }
            else
            {
                questiontype+=":FTP";
            }

            if(pd==0&&pddc==16)
            {
                questiontype+=":XD";
            }
            else
            {
                questiontype+=":XC";
            }

            if (isrightcount > 1)
            {
                questiontype += ":DX";
            }



            if (timu.Text != "")
            {
                var isrecord = from c in questionDataSet.questions where c.flag == flag[flagstep] select c;
                if (isrecord.Count() > 0)
                {
                    isrecord.First().flag += ":" + flagtxt.Text;
                    isrecord.First().driverlicensetype += ":" + cartypetxt;
                    imgsrc = "";
                }
                else
                {
                    questionDataSet.questions.AddquestionsRow(timu.Text, pd.ToString(), cartypetxt.Text, questiontype, 1, 4, 0, flag[flagstep], flagtxt.Text);
                }
                //questionDataSet.questions.AddquestionsRow(timu.Text, pd.ToString(), "C1", questiontype, 1, 4, 0);
                questionDataSetquestionsTableAdapter.Update(questionDataSet.questions);
                questionDataSet.questions.AcceptChanges();
                questionDataSetquestionsTableAdapter.Fill(questionDataSet.questions);

                var ques = from c in questionDataSet.questions select c;
                int ques_id = ques.Last().id;
                imgid = ques_id;

                if (isrecord.Count() < 1)
                {
                    questionDataSet.answers.AddanswersRow(op1.Text, isright1, ques_id);
                    questionDataSet.answers.AddanswersRow(op2.Text, isright2, ques_id);
                    questionDataSet.answers.AddanswersRow(op3.Text, isright3, ques_id);
                    questionDataSet.answers.AddanswersRow(op4.Text, isright4, ques_id);
                    questionDataSetanswersTableAdapter.Update(questionDataSet.answers);
                    questionDataSet.answers.AcceptChanges();
                }
            }


            //////////////////////写入数据库结束////////////////////
            



            if (flagstep < flag.Count-1 && timu.Text!="")
            {
                flagstep++;
                try
                {
                    wb.Navigate(new Uri("http://www.jiakaobaodian.com/tiku/shiti/" + flag[flagstep] + ".html"));
                }
                catch
                {
                    //step -= 100;
                    wb.Navigate(new Uri("http://www.jiakaobaodian.com/tiku/shiti/" + flag[flagstep] + ".html"));
                }
                if (imgsrc != "" && timu.Text != "")
                {
                    downfile(imgsrc, imgid);
                }
                timer_count = 0;
            }
            else
            {
                timer.Stop();
                System.Windows.MessageBox.Show("处理完成！");
            }

        }


        private void downfile(string url,int imgid)
        {
                        Dispatcher.Invoke(new Action(delegate
            {
            string[] filenames = url.Split('/');
            string[] filenamearr = filenames.Last().Split('.');
            string filename = imgid.ToString() + "." + filenamearr.Last();
            WebClient download = new WebClient();
            download.DownloadFileAsync(new Uri(url), "picture/"+filename);
            }));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            getHTML.questionDataSet questionDataSet = ((getHTML.questionDataSet)(this.FindResource("questionDataSet")));
            // 将数据加载到表 questions 中。可以根据需要修改此代码。
            getHTML.questionDataSetTableAdapters.questionsTableAdapter questionDataSetquestionsTableAdapter = new getHTML.questionDataSetTableAdapters.questionsTableAdapter();
            questionDataSetquestionsTableAdapter.Fill(questionDataSet.questions);
            System.Windows.Data.CollectionViewSource questionsViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("questionsViewSource")));
            questionsViewSource.View.MoveCurrentToFirst();
            // 将数据加载到表 answers 中。可以根据需要修改此代码。
            getHTML.questionDataSetTableAdapters.answersTableAdapter questionDataSetanswersTableAdapter = new getHTML.questionDataSetTableAdapters.answersTableAdapter();
            questionDataSetanswersTableAdapter.Fill(questionDataSet.answers);
            System.Windows.Data.CollectionViewSource answersViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("answersViewSource")));
            answersViewSource.View.MoveCurrentToFirst();
            
            timer.Interval = 1000;
            timer.Tick += new EventHandler(timer_Tick);
            //timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            timer_count++;
            if (timer_count > 60)
            {
                //getbtn_Click(null, null);
                clickwb();
                timer_count = 0;
            }
        }

        private void wblist_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            string title;//题名
            string questiontype="";//题型
            string a1;
            string a2;
            string a3;
            string a4;
            int isright1=0;
            int isright2=0;
            int isright3=0;
            int isright4=0;
            string myflag;
            string temanswer;
            string imgsrc;
            int juade = 0;
            if (e.Url.ToString() != wblist.Url.ToString())
                return;
            HtmlElementCollection a = wblist.Document.GetElementsByTagName("a");
            foreach (HtmlElement aelem in a)
            {
                title = "";
                questiontype = "";
                a1 = "";
                a2 = "";
                a3 = "";
                a4 = "";
                isright1 = 0;
                isright2 = 0;
                isright3 = 0;
                isright4 = 0;
                temanswer = "";
                myflag = "";
                imgsrc = "";
                if (aelem.GetAttribute("className").Equals("a-dis"))
                {
                    myflag = aelem.GetAttribute("href");
                    string[] tem1 = myflag.Split('/');
                    string[] tem2 =tem1.Last().Split('.');
                    myflag = tem2[0];
                    //string tem_href = aelem.GetAttribute("href");
                    //string[] hrefarr = tem_href.Split('/');
                    //string[] flagarr = hrefarr.Last().Split('.');
                    //flag.Add(flagarr[0]);
                    //HtmlElementCollection h4 = wblist.Document.GetElementsByTagName("h4");

                    HtmlElementCollection h4 = aelem.GetElementsByTagName("h4");
                    if (h4[0].OuterText.ToString().Substring(0, 1) == "单")
                    {
                        questiontype = "XZ";
                    }
                    else if (h4[0].OuterText.ToString().Substring(0, 1) == "判")
                    {
                        questiontype = "PD";
                    }
                    else if (h4[0].OuterText.ToString().Substring(0, 1) == "多")
                    {
                        questiontype = "XZ:DX";
                    }
                    title = h4[0].OuterText.ToString().Substring(2, h4[0].OuterText.ToString().Length - 2);


                    if (questiontype.Contains("XZ"))
                    {
                        juade=1;
                        HtmlElementCollection p = aelem.GetElementsByTagName("p");
                        a1 = p[0].OuterText.ToString().Substring(2, p[0].OuterText.ToString().Length - 2);
                        a2 = p[1].OuterText.ToString().Substring(2, p[1].OuterText.ToString().Length - 2);
                        a3 = p[2].OuterText.ToString().Substring(2, p[2].OuterText.ToString().Length - 2);
                        a4 = p[3].OuterText.ToString().Substring(2, p[3].OuterText.ToString().Length - 2);
                        HtmlElementCollection div = aelem.GetElementsByTagName("div");
                        foreach(HtmlElement divcontent in div)
                        {
                            if (divcontent.GetAttribute("className").Equals("answer"))
                            {
                                temanswer = divcontent.OuterText.ToString().Substring(5,divcontent.OuterText.ToString().Length-5);
                                temanswer = temanswer.Replace("查看解析", "");
                                if (temanswer.Contains("A"))
                                {
                                    isright1 = 1;
                                }
                                if (temanswer.Contains("B"))
                                {
                                    isright2 = 1;
                                }
                                if (temanswer.Contains("C"))
                                {
                                    isright3 = 1;
                                }
                                if (temanswer.Contains("D"))
                                {
                                    isright4 = 1;
                                }
                            }
                        }
                    }
                    else if (questiontype.Contains("PD"))
                    {
                        juade=0;
                        a1 = "正确";
                        a2 = "错误";
                        HtmlElementCollection div = aelem.GetElementsByTagName("div");
                        foreach (HtmlElement divcontent in div)
                        {
                            if (divcontent.GetAttribute("className").Equals("answer"))
                            {
                                temanswer = divcontent.OuterText.ToString().Substring(5, divcontent.OuterText.ToString().Length - 5);
                                temanswer = temanswer.Replace("查看解析", "");
                                if (temanswer.Contains("正确"))
                                {
                                    isright1 = 1;
                                    questiontype += ":XD";
                                }
                                if (temanswer.Contains("错误"))
                                {
                                    isright2 = 1;
                                    questiontype += ":XC";
                                }
                            }
                        }
                    }















                    HtmlElementCollection img = aelem.GetElementsByTagName("img");
                    if (img.Count > 0)
                    {
                        imgsrc = img[0].GetAttribute("src");
                        questiontype += ":TP";
                        //downfile(img[0].GetAttribute("src"), 0);
                    }
                    else
                    {
                        questiontype += ":FTP";
                    }



                    getHTML.questionDataSet questionDataSet = ((getHTML.questionDataSet)(this.FindResource("questionDataSet")));
                    // 将数据加载到表 questions 中。可以根据需要修改此代码。
                    getHTML.questionDataSetTableAdapters.questionsTableAdapter questionDataSetquestionsTableAdapter = new getHTML.questionDataSetTableAdapters.questionsTableAdapter();
                    questionDataSetquestionsTableAdapter.Fill(questionDataSet.questions);

                    getHTML.questionDataSetTableAdapters.answersTableAdapter questionDataSetanswersTableAdapter = new getHTML.questionDataSetTableAdapters.answersTableAdapter();
                    questionDataSetanswersTableAdapter.Fill(questionDataSet.answers);

                    var haveques = from c in questionDataSet.questions where c.para == myflag select c;
                    if (haveques.Count() > 0)
                    {
                        haveques.First().driverlicensetype += ":" + cartypetxt.Text;
                        haveques.First().flag += ":" + flagtxt.Text;
                        questionDataSetquestionsTableAdapter.Update(questionDataSet.questions);
                        questionDataSet.questions.AcceptChanges();
                        questionDataSetquestionsTableAdapter.Fill(questionDataSet.questions);
                    }
                    else
                    {
                        questionDataSet.questions.AddquestionsRow(title, juade.ToString(), cartypetxt.Text, questiontype, 1, 4, 0, myflag, flagtxt.Text);
                        questionDataSetquestionsTableAdapter.Update(questionDataSet.questions);
                        questionDataSet.questions.AcceptChanges();
                        questionDataSetquestionsTableAdapter.Fill(questionDataSet.questions);
                        int ques_id = (from c in questionDataSet.questions select c).Last().id;
                        questionDataSet.answers.AddanswersRow(a1, isright1, ques_id);
                        questionDataSet.answers.AddanswersRow(a2, isright2, ques_id);
                        if (questiontype.Contains("XZ"))
                        {
                            questionDataSet.answers.AddanswersRow(a3, isright3, ques_id);
                            questionDataSet.answers.AddanswersRow(a4, isright4, ques_id);
                        }
                        questionDataSetanswersTableAdapter.Update(questionDataSet.answers);
                        questionDataSet.answers.AcceptChanges();
                        if (imgsrc != "")
                        {
                            downfile(imgsrc, ques_id);
                        }
                    }
                }


                



            }
            step++;
            if (step < lastpages-1)
            {
                wblist.Navigate(new Uri(linktxt.Text + pages[step]));
            }
            else
            {
                //flag.Remove("1100000");
                //flagstep++;
                //wb.Visible = true;
                //wblist.Visible = false;
                //wblisthost.Visibility = Visibility.Collapsed;
                //timer.Start();
                //clickwb();
                System.Windows.MessageBox.Show("处理完成");
            }
        }

        //修改总页数 代码行数41行366行
        //修改链接地址 代码行数46行
        //修改页数
        //修改章节flag 246行251行

    }
}
