using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NaiveBayesForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void CheckSentiment_Click(object sender, EventArgs e)
        {
            NaiveBayes nb = new NaiveBayes();
             string input = inputField.Text;
            if (input == string.Empty)
            {
                MessageBox.Show("Sorry you did not enter any sentence", "Error Message", MessageBoxButtons.OK);
            }
            else
            {
                try
                {
                    StreamReader srneg = new StreamReader("NegativeFile.txt");
                    StreamReader srpos = new StreamReader("PositiveFile.txt");
                    string line = "";
                    while (line != null)
                    {
                        line = srneg.ReadLine();
                        if (line != null)
                        {
                            NaiveBayes.TrainingSet.Add(new Document("negative", line));
                        }
                    }
                    srneg.Close();
                    string line2 = "";
                    while (line2 != null)
                    {
                        line2 = srpos.ReadLine();
                        if (line2 != null)
                        {
                            NaiveBayes.TrainingSet.Add(new Document("positive", line2));
                        }

                    }
                    srpos.Close();
                    input = inputField.Text;
                    var c = new Classifier(NaiveBayes.TrainingSet);
                    var neg = c.IsInClassProbability("negative", input);
                    var pos = c.IsInClassProbability("positive", input);
                    label4.Text = neg + "";

                    label5.Text = pos + "";
                    //label3.Text = c.IsInClassProbability("positive", input)+"";
                    if (neg > pos)
                    {
                        richTextBox1.Text = "The statement: '" + input +"'\n"+ " has a " + "Negative Tone";
                        //label4.Text = neg + "";
                        //Console.WriteLine("The statement has a negative tone");
                    }
                    else
                    {
                        richTextBox1.Text = "The statement: '"+ input + "'\n"+ " has a " + "positive tone";
                        //label5.Text = pos + "";
                        // Console.WriteLine("The statement has a positive tone");

                    }


                }
                catch (Exception ex)
                {
                    MessageBox.Show("Invalid input!", ex.ToString(), MessageBoxButtons.OK);
                    return;
                }
            }
            inputField.Text = string.Empty;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
