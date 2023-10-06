using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Numpy;

namespace Hackaton2023
{
    class Demo
    {
        const int TRAINING_SIZE = 10;
        const int TEST_SIZE = 10;

        public void Start()
        {
            var random = new Random();

            // build training data
            ////////////////////////////////////////////////////////////////////////////////////////
            ///
            BuildTrainingData(TRAINING_SIZE, out var x_train_bytes, out var y_train_bytes, true);
            BuildTrainingData(TEST_SIZE, out var x_test_bytes, out var y_test_bytes, false);


            // train the model
            ////////////////////////////////////////////////////////////////////////////////////////
            var bot = new SupportBot();

            var x_train = (new NDarray(x_train_bytes)).reshape(TRAINING_SIZE, SupportBot.SEQUENCE_LENGTH, 1);
            var y_train = (new NDarray(y_train_bytes)).reshape(TRAINING_SIZE, SupportBot.OUTPUT_SIZE); 
            bot.ModelTraining(x_train, y_train);
        }

        private void BuildTrainingData(int data_size, out byte[] x_train_bytes, out byte[] y_train_bytes, bool do_plot)
        {
            var random = new Random();
            x_train_bytes = new byte[data_size * SupportBot.SEQUENCE_LENGTH];
            y_train_bytes = new byte[data_size];

            for (int i = 0; i < data_size; i++)
            {                
                // randomize training type
                int type = random.Next(0, 3);
                switch (type)
                {
                    case 0:
                        {
                            var sample = SplunkSimulator.GetStraightTrainingSample();
                            sample.CopyTo(x_train_bytes, i * SupportBot.SEQUENCE_LENGTH);
                            y_train_bytes[i] = 0;
                            if(do_plot)
                                PlotSample(sample, $"demo_straight{i}.png");
                        }
                        break;
                    case 1:
                        {
                            var sample = SplunkSimulator.GetSpikeTrainingSample();
                            sample.CopyTo(x_train_bytes, i * SupportBot.SEQUENCE_LENGTH);
                            y_train_bytes[i] = 255;
                            PlotSample(sample, $"demo_spike{i}.png");
                        }
                        break;
                    case 2:
                        {
                            var sample = SplunkSimulator.GetCyclicTrainingSample();
                            sample.CopyTo(x_train_bytes, i * SupportBot.SEQUENCE_LENGTH);
                            y_train_bytes[i] = 0;
                            PlotSample(sample, $"demo_cyclic{i}.png");
                        }
                        break;
                }
            }
        }

        private void PlotSample(byte[] data, string file_name)
        {
            Bitmap bmp = null;
            Graphics gfx = null;
            Pen pen = null;
            try
            {
                bmp = new Bitmap(512, 512);
                gfx = Graphics.FromImage(bmp);
                gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                gfx.Clear(Color.White);

                int step = 512 / data.Length;
                step = (step == 0) ? 1 : step;

                pen = new Pen(Color.Navy);
                for (int i = 0; i < data.Length-1; i++)
                {                    
                    gfx.DrawLine(pen, i*step, 512 - data[i], (i+1)*step, 512 - data[i+1]);
                }

                bmp.Save(file_name);
            }
            finally
            {
                bmp?.Dispose();
                gfx?.Dispose();
                pen?.Dispose();
            }
        }
    }
}
