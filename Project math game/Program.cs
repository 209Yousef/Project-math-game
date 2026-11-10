using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Project_2_Math_Game
{
    internal class Program
    {

        enum enQuestionLevel { Easy = 1, Mid = 2, Hard = 3, MixLevel = 4 };

        enum enOperationType { Add = 1, Sub = 2, Mult = 3, Div = 4, MixOp = 5 };

        struct stQuestions
        {
            public int number1;
            public int number2;
            public enQuestionLevel questionLevel;
            public enOperationType operationType;
            public int correctAnswer;
            public int playerAnswer;
            public bool answerResult;
        }

        struct stQuizz
        {
            public stQuestions[] listQuestions;
            public short numberOfQuestions;
            public enQuestionLevel questionsLevel;
            public enOperationType opType;
            public short numberOfCorrectAnswer;
            public short numberOfWrongAnswer;
            public bool isPass;
        }

        static short ReadNumberOfQuestion()
        {
            short numberOfQuestion;

            do
            {
                Console.Write("How Many Questions Do You Want To Answer ? ");
                numberOfQuestion = short.Parse(Console.ReadLine());
            }
            while (numberOfQuestion < 1 || numberOfQuestion > 10);

            return numberOfQuestion;
        }

        static enQuestionLevel ReadQuestionLevel()
        {
            short questionLevel;

            do
            {
                Console.Write("\nPlease Enter The Question Level [1]:Easy, [2]:Mid, [3]:Hard, [4]:Mix ? ");
                questionLevel = short.Parse(Console.ReadLine());
            }
            while (questionLevel < 1 || questionLevel > 4);

            return (enQuestionLevel)questionLevel;
        }

        static enOperationType ReadOpType()
        {
            short opType;

            do
            {
                Console.Write("\nPlease Enter The Operation Type [1]:Add, [2]:Sub, [3]:Mult, [4]:Div, [5]:Mix ? ");
                opType = short.Parse(Console.ReadLine());
            }
            while (opType < 1 || opType > 5);

            return (enOperationType)opType;
        }

        static Random rnd = new Random();

        static int RandomNumber(int from, int to)
        {
            return rnd.Next(from, to + 1);
        }

        static int SimpleCalculater(int number1, int number2, enOperationType operationType)
        {
            switch (operationType)
            {
                case enOperationType.Add:
                    return number1 + number2;

                case enOperationType.Sub:
                    return number1 - number2;

                case enOperationType.Mult:
                    return number1 * number2;

                case enOperationType.Div:
                    return number1 / number2;

                default:
                    return number1 + number2;
            }
        }

        static stQuestions GenerateQuestion(enQuestionLevel questionLevel, enOperationType operationType)
        {
            stQuestions questions = new stQuestions();

            if (questionLevel == enQuestionLevel.MixLevel)
                questionLevel = (enQuestionLevel)RandomNumber(1, 3);

            if (operationType == enOperationType.MixOp)
                operationType = (enOperationType)RandomNumber(1, 4);

            switch (questionLevel)
            {
                case enQuestionLevel.Easy:
                    questions.number1 = RandomNumber(1, 10);
                    questions.number2 = RandomNumber(1, 10);

                    questions.questionLevel = questionLevel;
                    questions.operationType = operationType;

                    questions.correctAnswer = SimpleCalculater(questions.number1, questions.number2, questions.operationType);
                    return questions;

                case enQuestionLevel.Mid:
                    questions.number1 = RandomNumber(10, 50);
                    questions.number2 = RandomNumber(10, 50);

                    questions.questionLevel = questionLevel;
                    questions.operationType = operationType;

                    questions.correctAnswer = SimpleCalculater(questions.number1, questions.number2, questions.operationType);
                    return questions;

                case enQuestionLevel.Hard:
                    questions.number1 = RandomNumber(50, 100);
                    questions.number2 = RandomNumber(50, 100);

                    questions.questionLevel = questionLevel;
                    questions.operationType = operationType;

                    questions.correctAnswer = SimpleCalculater(questions.number1, questions.number2, questions.operationType);
                    return questions;
            }
            return questions;
        }

        static void GenerateQuizzQuestions(ref stQuizz quizz)
        {
            for (int questionNumber = 0; questionNumber < quizz.numberOfQuestions; questionNumber++)
            {
                quizz.listQuestions[questionNumber] = GenerateQuestion(quizz.questionsLevel, quizz.opType);
            }
        }

        static char GetSumbolOperation(enOperationType operationType)
        {
            switch (operationType)
            {
                case enOperationType.Add:
                    return '+';

                case enOperationType.Sub:
                    return '-';

                case enOperationType.Mult:
                    return '*';

                case enOperationType.Div:
                    return '/';

                default:
                    return '\0';
            }

        }

        static void PrintQuestions(stQuizz quizz, int question)
        {
            Console.WriteLine("\nQuestion [" + (question + 1) + "/" + quizz.numberOfQuestions + "]");
            Console.WriteLine();
            Console.WriteLine(quizz.listQuestions[question].number1);
            Console.WriteLine("\t" + GetSumbolOperation(quizz.listQuestions[question].operationType));
            Console.WriteLine(quizz.listQuestions[question].number2);
            Console.WriteLine("----------");
        }

        static void SetColorScreen(stQuizz quizz, int question)
        {
            if (quizz.listQuestions[question].correctAnswer == quizz.listQuestions[question].playerAnswer)
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Beep();
            }
            Console.Clear();
        }

        static void CorrectAnswerResult(ref stQuizz quizz, int question)
        {
            SetColorScreen(quizz, question);

            if (quizz.listQuestions[question].correctAnswer == quizz.listQuestions[question].playerAnswer)
            {
                Console.WriteLine("Right Answer :-)");
                quizz.numberOfCorrectAnswer++;
            }
            else
            {
                Console.WriteLine("Wrong Answer :-(");
                Console.WriteLine("The Correct Answer Is: " + quizz.listQuestions[question].correctAnswer);
                quizz.numberOfWrongAnswer++;
            }

        }

        static int ReadPlayerAnswer()
        {
            int playerAnswer;

            playerAnswer = int.Parse(Console.ReadLine());

            return playerAnswer;
        }

        static void AskAndCorrectQuizzAnswer(ref stQuizz quizz)
        {
            for (int question = 0; question < quizz.numberOfQuestions; question++)
            {
                PrintQuestions(quizz, question);

                quizz.listQuestions[question].playerAnswer = ReadPlayerAnswer();

                CorrectAnswerResult(ref quizz, question);
            }
            quizz.isPass = quizz.numberOfCorrectAnswer >= quizz.numberOfWrongAnswer;
        }

        static string GetQuestionLevel(enQuestionLevel questionLevel)
        {
            string[] arrStringQuestionLevel = { "Easy", "Miduem", "Hard", "Mix" };

            return arrStringQuestionLevel[(short)questionLevel - 1];
        }

        static string GetOperationType(enOperationType operationType)
        {
            string[] arrStringOperationType = { "Add", "Sub", "Mult", "Div", "Mix" };

            return arrStringOperationType[(short)operationType - 1];
        }

        static string GetEmoji(stQuizz quizz)
        {
            if (quizz.isPass == true)
                return ":-)";
            else
                return ":-(";
        }

        static void PrintQuizz(stQuizz quizz)
        {
            Console.WriteLine("\n\n--------------------------------");
            Console.WriteLine("    Final Result Is: " + quizz.isPass + GetEmoji(quizz));
            Console.WriteLine("--------------------------------");
            Console.WriteLine("\nNumber Of Question: " + quizz.numberOfQuestions);
            Console.WriteLine("Questions Level: " + GetQuestionLevel(quizz.questionsLevel));
            Console.WriteLine("Operation Type: " + GetOperationType(quizz.opType));
            Console.WriteLine("Number Of Correct Answer: " + quizz.numberOfCorrectAnswer);
            Console.WriteLine("Number Of Wrong Answer: " + quizz.numberOfWrongAnswer);
            Console.WriteLine("Is Pass: " + quizz.isPass);
            Console.WriteLine("--------------------------------");
        }

        static void PlayMathGame()
        {
            stQuizz quizz = new stQuizz();

            quizz.numberOfQuestions = ReadNumberOfQuestion();
            quizz.questionsLevel = ReadQuestionLevel();
            quizz.opType = ReadOpType();

            quizz.listQuestions = new stQuestions[100];
            GenerateQuizzQuestions(ref quizz);

            AskAndCorrectQuizzAnswer(ref quizz);

            PrintQuizz(quizz);
        }

        static void ResetScrean()
        {
            Console.ResetColor();
            Console.Clear();
        }

        static void StartGame()
        {
            char playAgain;

            do
            {
                ResetScrean();

                PlayMathGame();

                Console.Write("\nDo You Want Play Again [y]:Yes, [n]:No ? ");
                playAgain = char.Parse(Console.ReadLine());
            }
            while (playAgain == 'y' || playAgain == 'Y');
        }

        static void Main(string[] args)
        {
            StartGame();
        }
    }
}

