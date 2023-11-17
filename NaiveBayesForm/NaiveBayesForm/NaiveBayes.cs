using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NaiveBayesForm
{
    class NaiveBayes
    {

        public static List<Document> TrainingSet = new List<Document>
        {
            

        };
       }
    //This class is responsible for reading a document of words
    class Document
    {
        //This constructor is responsible for passing text and also the class of the text i.e either positive or negative
        public Document(string @class, string text)
        {
            Class = @class;
            Text = text;
        }
        //getter and setter methods for the document class
        public string Class { get; set; }
        public string Text { get; set; }

    }


    public static class Helpers
    {
        //This method is responsible for extracting features in a given sentence and storing them in a list
        public static List<String> ExtractFeatures(this String text)
        {
            //Data cleaning
            return Regex.Replace(text.ToLower(), "\\p{P}+", "").Split(' ').ToList();


        }
    }
    class ClassInfo //This class is responsible for getting all the words, tokenizing them and specifying the number of times each of the tokenized word occurs
    {
        public ClassInfo(string name, List<String> trainDocs)
        {
            Name = name;
            var features = trainDocs.SelectMany(x => x.ExtractFeatures());// get all the tokenuzed words from the documents trined, selects the extracted features and stores them in features
            WordsCount = features.Count();// gets the number of  all the extracted words 
            //gets all the words that have been tokenized, group them by their class and 
            WordCount = features.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
            // int x = NumberOfDocs.GetType(y)
            // get all the words to be trained, group each word according to the number of times they occur
            NumberOfDocs = trainDocs.Count;
        }
        public string Name { get; set; }
        public int WordsCount { get; set; }
        //getter and setter methods key value pair of words in the dictionary
        public Dictionary<string, int> WordCount { get; set; }
        //getter and setter methods for counting the number of documents in a training set
        public int NumberOfDocs { get; set; }
        //This method ia counting the number of times a particular word appears
        public int NumberOfOccurencesInTrainDocs(String word)
        {
            if (WordCount.Keys.Contains(word))
            {
                return WordCount[word];
            }
            return 0;
        }
    }
    class Classifier // This class is responsible for getting the documents and training each words according to the class they fall into.
    {
        List<ClassInfo> classes;
        int countOfDocs;
        int uniqWordsCount;
        //constructor accepts d document and classify them
        public Classifier(List<Document> train)
        {
            classes = train.GroupBy(x => x.Class).Select(g => new ClassInfo(g.Key, g.Select(x => x.Text).ToList())).ToList();
            countOfDocs = train.Count;//counts all the words in the document list
            uniqWordsCount = train.SelectMany(x => x.Text.Split(' ')).GroupBy(x => x).Count();
        }
        public double IsInClassProbability(string className, string text)
        {

            //extract features from the text entered and store it in variable words
            var words = text.ExtractFeatures();

            var classResults = classes.Select(x => new { Result = Math.Pow(Math.E, Calc(x.NumberOfDocs, countOfDocs, words, x.WordsCount, x, uniqWordsCount)), ClassName = x.Name });
            return classResults.Single(x => x.ClassName == className).Result / classResults.Sum(x => x.Result);
        }
        //Naive Bayes Model
        private static double Calc(double words, double NumofWordOccurences, List<String> q, double numOfwordsInCategory, ClassInfo @class, double totalnumofwordsincat)
        {
            //likelihood P(X|c) = (count(x,c) + 1)/count(c) + |v|
            //naive bayes = Log(P(Class)) + sum(Log(P(X|Class)))
            return Math.Log(words / NumofWordOccurences) + q.Sum(x => Math.Log((@class.NumberOfOccurencesInTrainDocs(x) + 1) / (totalnumofwordsincat + numOfwordsInCategory)));
        }
    }
}
