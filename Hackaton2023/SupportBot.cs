using Keras;
using Keras.Layers;
using Keras.Models;
using Numpy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackaton2023
{
    class SupportBot
    {
        public const int BATCH_SIZE = 10;
        public const int EPOCHS = 300;
        public const int SEQUENCE_LENGTH = 128;
        public const int ROW_SIZE = 83;
        public const int OUTPUT_SIZE = 1;

        public void ModelTraining(NDarray x_train, NDarray y_train)
        {
            var model = new Sequential();

            model.Add(new LSTM(32, input_shape: new Shape(SEQUENCE_LENGTH, 1)));
            model.Add(new Dense(OUTPUT_SIZE, activation: "softmax"));  // output of the form: [ no issue, latency, failed transactions]

            model.Compile(optimizer: "sgd", loss: "categorical_crossentropy", metrics: new string[] { "accuracy" });
            model.Summary();
            
            model.Fit(x_train, y_train, BATCH_SIZE, EPOCHS, 1);
        }

        public (byte[,,], byte[,]) ReadData(string file_name)
        {
            return (new byte[ROW_SIZE, SEQUENCE_LENGTH, 10], new byte[OUTPUT_SIZE, 10]);
        }       
    }
}
