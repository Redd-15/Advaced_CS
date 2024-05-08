using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Controls.Ribbon;

namespace Telemetry_app
{
    public class TelemetryModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler? PropertyChanged;
        public string RawData { get; set; }
        private string[] RawArrayData { get; set; }
        private double[] ParsedArrayData { get; set; }

        private readonly static int[] GREEN = { 0, 255, 0 };
        private readonly static int[] RED   = { 255, 0, 0 };
        private readonly static int[] BLUE  = { 0, 0, 255 };
        private readonly static int[] WHITE = { 255, 255, 255 };
        private readonly static int[] HV_YELLOW = { 255, 255, 0 };
        private readonly static int[] LV_ORANGE = { 255, 200, 0 };

        //TEMPS
        public string Temp_FrontLeft_Color { get; set; } //0
        public double Temp_FrontLeft { get; set; }
        public string Temp_FrontRight_Color { get; set; }  //1
        public double Temp_FrontRight { get; set; }
        public string Temp_RearLeft_Color { get; set; }  //2
        public double Temp_RearLeft { get; set; }
        public string Temp_RearRight_Color { get; set; }  //3
        public double Temp_RearRight { get; set; }

        public string Temp_Motor_Color { get; set; }  //4
        public double Temp_Motor { get; set; }
        public string Temp_Coolant_Color { get; set; }  //5
        public double Temp_Coolant { get; set; }
        public string Temp_HVBatt_Color { get; set; } //6
        public double Temp_HVBatt { get; set; }
        public string Temp_LVBatt_Color { get; set; }  //7
        public double Temp_LVBatt { get; set; }
        public string Temp_Inverter_Color { get; set; }  //8
        public double Temp_Inverter { get; set; }


        private double Temp_FSU = 0;  //9
        public double Temp_FSU_Current { get; set; }
        public string Temp_FSU_Color { get; set; }

        private double Temp_RSU = 0;  //10
        public double Temp_RSU_Current { get; set; }
        public string Temp_RSU_Color { get; set; }

        private double Temp_MCU = 0;  //11
        public double Temp_MCU_Current { get; set; }
        public string Temp_MCU_Color { get; set; }

        //CONTROLS
        private double Cont_Wheel;  //12
        public double Cont_Wheel_Current { get; set; }

        private double Cont_ACC;  //13
        public double Cont_ACC_Current { get; set; }
        private double Cont_BRK;  //14
        public double Cont_BRK_Current { get; set; }

        //SPEED
        public double Speed_FL { get; set; }  //15
        public double Speed_FR { get; set; }  //15
        public double Speed_RL { get; set; }  //15
        public double Speed_RR { get; set; }  //15
        public double Speed_AVG { get; set; }  //15

        //ELEC
        public double Elec_HVAmp { get; set; }  //16
        public double Elec_HVVolt { get; set; }  //17
        public double Elec_HVCap { get; set; }
        public string Elec_HVCap_Color { get; set; }

        public double Elec_LVAmp { get; set; }  //16
        public double Elec_LVVolt { get; set; }  //17
        public double Elec_LVCap { get; set; }
        public string Elec_LVCap_Color { get; set; }

        //COOLING
        private double Cool_Fan1 = 0;  //18
        public double Cool_Fan1_Current { get; set; }
        public string Cool_Fan1_Color { get; set; }
        private double Cool_Fan2 = 0;  //18
        public double Cool_Fan2_Current { get; set; }
        public string Cool_Fan2_Color { get; set; }
        private double Cool_Fan3 = 0;  //18
        public double Cool_Fan3_Current { get; set; }
        public string Cool_Fan3_Color { get; set; }

        //GFORCE
        private double GF_X;  //19
        private double GF_Y;  //20

        private double GF_X_Current = 0.0;
        private double GF_Y_Current = 0.0;
        public string GF { get; set; } = "0,0,0,0";

        public void SetData(string data) {

            RawData = data.Split('\"')[1] + ';';
            RawArrayData = RawData.Split(';');

            ParsedArrayData = new double[RawArrayData.Length];

            for (int i = 0; i < RawArrayData.Length - 1; i++)
            {
                if (!double.TryParse(RawArrayData[i], out ParsedArrayData[i])) { throw new Exception("Array parse error at index: " + i + " Len of RawArray:" + RawArrayData.Length); }
            }

            //Tyre temps
            Temp_FrontLeft = ParsedArrayData[0];
            Temp_FrontLeft_Color = ColorInterpolation(BLUE, GREEN, RED, 0, 200, Temp_FrontLeft);
            Temp_FrontRight = ParsedArrayData[1];
            Temp_FrontRight_Color = ColorInterpolation(BLUE, GREEN, RED, 0, 200, Temp_FrontRight);
            Temp_RearLeft = ParsedArrayData[2];
            Temp_RearLeft_Color = ColorInterpolation(BLUE, GREEN, RED, 0, 200, Temp_RearLeft);
            Temp_RearRight = ParsedArrayData[3];
            Temp_RearRight_Color = ColorInterpolation(BLUE, GREEN, RED, 0, 200, Temp_RearRight);

            //Elec temps
            Temp_Motor = ParsedArrayData[4];
            Temp_Motor_Color = ColorInterpolation(GREEN, RED, 0, 500, Temp_Motor);
            Temp_Coolant = ParsedArrayData[5];
            Temp_Coolant_Color = ColorInterpolation(GREEN, RED, 0, 500, Temp_Coolant);
            Temp_HVBatt = ParsedArrayData[6];
            Temp_HVBatt_Color = ColorInterpolation(GREEN, RED, 0, 100, Temp_HVBatt);
            Temp_LVBatt = ParsedArrayData[7];
            Temp_LVBatt_Color = ColorInterpolation(GREEN, RED, 0, 100, Temp_LVBatt);
            Temp_Inverter = ParsedArrayData[8];
            Temp_Inverter_Color = ColorInterpolation(GREEN, RED, 0, 100, Temp_Inverter);

            Temp_FSU = LinearInterpolation(0, 400, 0, 100, ParsedArrayData[9]);
            Temp_RSU = LinearInterpolation(0, 400, 0, 100, ParsedArrayData[10]);
            Temp_MCU = LinearInterpolation(0, 400, 0, 100, ParsedArrayData[11]);

            //Controls
            //Cont_Wheel = LinearInterpolation(-180,180,-180,180, ParsedArrayData[12]); //if interpolation is needed!
            Cont_Wheel = ParsedArrayData[12];
            Cont_ACC = LinearInterpolation(0, 278, 0, 100, ParsedArrayData[13]);
            Cont_BRK = LinearInterpolation(0, 278, 0, 100, ParsedArrayData[14]);

            //Speeds
            Speed_AVG = ParsedArrayData[15];
            Speed_FL = ParsedArrayData[15];
            Speed_FR = ParsedArrayData[15];
            Speed_RL = ParsedArrayData[15];
            Speed_RR = ParsedArrayData[15];

            Elec_HVAmp = ParsedArrayData[16];
            Elec_LVAmp = ParsedArrayData[16]/10;
            Elec_HVVolt = ParsedArrayData[17];
            Elec_LVVolt = ParsedArrayData[17]/42;
            
            Cool_Fan1 = LinearInterpolation(0, 400 ,0 , 100, ParsedArrayData[18]);
            Cool_Fan2 = LinearInterpolation(0, 400, 0, 100, ParsedArrayData[18]);
            Cool_Fan3 = LinearInterpolation(0, 400, 0, 100, ParsedArrayData[18]);


            //GF
            GF_X = (int)MinMax(-130, 130,ParsedArrayData[19]);
            GF_Y = (int)MinMax(-130, 130, ParsedArrayData[20]);

            //All the PropertyChanged event
            if (PropertyChanged != null)  
            {
                PropertyChanged(this, new PropertyChangedEventArgs("RawData"));
                PropertyChanged(this, new PropertyChangedEventArgs("RawArrayData"));

                PropertyChanged(this, new PropertyChangedEventArgs("Temp_FrontLeft_Color"));
                PropertyChanged(this, new PropertyChangedEventArgs("Temp_FrontRight_Color"));
                PropertyChanged(this, new PropertyChangedEventArgs("Temp_RearLeft_Color"));
                PropertyChanged(this, new PropertyChangedEventArgs("Temp_RearRight_Color"));
                PropertyChanged(this, new PropertyChangedEventArgs("Temp_Motor_Color"));
                PropertyChanged(this, new PropertyChangedEventArgs("Temp_Coolant_Color"));
                PropertyChanged(this, new PropertyChangedEventArgs("Temp_HVBatt_Color"));
                PropertyChanged(this, new PropertyChangedEventArgs("Temp_LVBatt_Color"));
                PropertyChanged(this, new PropertyChangedEventArgs("Temp_Inverter_Color"));

                PropertyChanged(this, new PropertyChangedEventArgs("Temp_FrontLeft"));
                PropertyChanged(this, new PropertyChangedEventArgs("Temp_FrontRight"));
                PropertyChanged(this, new PropertyChangedEventArgs("Temp_RearLeft"));
                PropertyChanged(this, new PropertyChangedEventArgs("Temp_RearRight"));
                PropertyChanged(this, new PropertyChangedEventArgs("Temp_Motor"));
                PropertyChanged(this, new PropertyChangedEventArgs("Temp_Coolant"));
                PropertyChanged(this, new PropertyChangedEventArgs("Temp_HVBatt"));
                PropertyChanged(this, new PropertyChangedEventArgs("Temp_LVBatt"));
                PropertyChanged(this, new PropertyChangedEventArgs("Temp_Inverter"));

                PropertyChanged(this, new PropertyChangedEventArgs("Speed_FL"));
                PropertyChanged(this, new PropertyChangedEventArgs("Speed_FR"));
                PropertyChanged(this, new PropertyChangedEventArgs("Speed_RL"));
                PropertyChanged(this, new PropertyChangedEventArgs("Speed_RR"));
                PropertyChanged(this, new PropertyChangedEventArgs("Speed_AVG"));

                PropertyChanged(this, new PropertyChangedEventArgs("Elec_HVAmp"));
                PropertyChanged(this, new PropertyChangedEventArgs("Elec_HVVolt"));
                PropertyChanged(this, new PropertyChangedEventArgs("Elec_LVAmp"));
                PropertyChanged(this, new PropertyChangedEventArgs("Elec_LVVolt"));

            }
            
        }

        public void CalculatePhysics(long timeDelta)
        {

            try
            {
                GF_X_Current += ((GF_X - GF_X_Current) * 200.0) * ((double)timeDelta / 10000000);
                GF_Y_Current += ((GF_Y - GF_Y_Current) * 200.0) * ((double)timeDelta / 10000000);

                Cont_Wheel_Current += ((Cont_Wheel - Cont_Wheel_Current) * 100.0) * ((double)timeDelta / 10000000);

                Cont_ACC_Current += ((Cont_ACC - Cont_ACC_Current) * 150.0) * ((double)timeDelta / 10000000);
                Cont_BRK_Current += ((Cont_BRK - Cont_BRK_Current) * 150.0) * ((double)timeDelta / 10000000);

                Temp_MCU_Current += ((Temp_MCU - Temp_MCU_Current) * 50.0) * ((double)timeDelta / 10000000);
                Temp_FSU_Current += ((Temp_FSU - Temp_FSU_Current) * 50.0) * ((double)timeDelta / 10000000);
                Temp_RSU_Current += ((Temp_RSU - Temp_RSU_Current) * 50.0) * ((double)timeDelta / 10000000);

                Cool_Fan1_Current += ((Cool_Fan1 - Cool_Fan1_Current) * 50.0) * ((double)timeDelta / 10000000);
                Cool_Fan2_Current += ((Cool_Fan2 - Cool_Fan2_Current) * 50.0) * ((double)timeDelta / 10000000);
                Cool_Fan3_Current += ((Cool_Fan3 - Cool_Fan3_Current) * 50.0) * ((double)timeDelta / 10000000);

                Elec_LVCap += ((LinearInterpolation(0, 400, 400 / 42, 480 / 42, Elec_LVVolt) - Elec_LVCap) * 50.0) * ((double)timeDelta / 10000000);
                Elec_HVCap += ((LinearInterpolation(0, 400, 400, 480, Elec_HVVolt) - Elec_HVCap) * 50.0) * ((double)timeDelta / 10000000);
            }
            catch (ArithmeticException)
            {
                GF_X_Current = GF_X;
                GF_Y_Current = GF_Y;

                Cont_Wheel_Current = Cont_Wheel;
                Cont_ACC_Current = Cont_ACC;
                Cont_BRK_Current = Cont_BRK;

                Temp_MCU_Current = Temp_MCU;
                Temp_FSU_Current = Temp_FSU;
                Temp_RSU_Current = Temp_RSU;

                Cool_Fan1_Current = Cool_Fan1;
                Cool_Fan2_Current = Cool_Fan2;
                Cool_Fan3_Current = Cool_Fan3;

                Elec_LVCap = (LinearInterpolation(0, 400, 400 / 42, 480 / 42, Elec_LVVolt));
                Elec_HVCap = (LinearInterpolation(0, 400, 400, 480, Elec_HVVolt));
            }

            GF = Math.Round(GF_X_Current + 174).ToString() + ",0,0," + Math.Round(GF_Y_Current + 173).ToString();
            Cont_ACC_Current = MinMax(0, 278, Cont_ACC_Current);
            Cont_BRK_Current = MinMax(0, 278, Cont_BRK_Current);

            Temp_MCU_Current = MinMax(0, 400, Temp_MCU_Current);
            Temp_FSU_Current = MinMax(0, 400, Temp_FSU_Current);
            Temp_RSU_Current = MinMax(0, 400, Temp_RSU_Current);
            Cool_Fan1_Current = MinMax(0, 400, Cool_Fan1_Current);
            Cool_Fan2_Current = MinMax(0, 400, Cool_Fan2_Current);
            Cool_Fan3_Current = MinMax(0, 400, Cool_Fan3_Current);

            if (PropertyChanged != null) {

                Temp_MCU_Color = ChangeColorIfMoreThan(Temp_MCU_Current, LinearInterpolation(0, 400, 0, 100, 80), WHITE, RED, "Temp_MCU_Color");
                Temp_FSU_Color = ChangeColorIfMoreThan(Temp_FSU_Current, LinearInterpolation(0, 400, 0, 100, 80), WHITE, RED, "Temp_FSU_Color");
                Temp_RSU_Color = ChangeColorIfMoreThan(Temp_RSU_Current, LinearInterpolation(0, 400, 0, 100, 80), WHITE, RED, "Temp_RSU_Color");

                Cool_Fan1_Color = ChangeColorIfMoreThan(Cool_Fan1_Current, LinearInterpolation(0, 400, 0, 100, 80), WHITE, RED, "Cool_Fan1_Color");
                Cool_Fan2_Color = ChangeColorIfMoreThan(Cool_Fan2_Current, LinearInterpolation(0, 400, 0, 100, 80), WHITE, RED, "Cool_Fan2_Color");
                Cool_Fan3_Color = ChangeColorIfMoreThan(Cool_Fan3_Current, LinearInterpolation(0, 400, 0, 100, 80), WHITE, RED, "Cool_Fan3_Color");

                Elec_HVCap_Color = ChangeColorIfLessThan(Elec_HVVolt, 420, HV_YELLOW, RED, "Elec_HVCap_Color");
                Elec_LVCap_Color = ChangeColorIfLessThan(Elec_LVVolt, 9.5, LV_ORANGE, RED, "Elec_LVCap_Color");

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("GF"));

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Cont_Wheel_Current"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Cont_ACC_Current"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Cont_BRK_Current"));

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Temp_MCU_Current"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Temp_FSU_Current"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Temp_RSU_Current"));

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Cool_Fan1_Current"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Cool_Fan2_Current"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Cool_Fan3_Current"));

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Elec_HVCap"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Elec_LVCap"));
            }

        }

        private static string ColorInterpolation(int[] colorMin, int[] colorMax, int minVal, int maxVal, double valIn)
        {
            int val = (int)valIn;

            int[] color = new int[3];

            if (val > maxVal)
            {
                val = maxVal;
            }
            else if (val < minVal)
            {
                val = minVal;
            }

            for (int i = 0; i < color.Length; i++)
            {
                color[i] = (val - minVal) * (colorMax[i] - colorMin[i]) / (maxVal - minVal) + colorMin[i];
                         //(x   - in_min) * (out_max   - out_min  ) / (in_max - in_min) + out_min;
            }

            return String.Format("#{0:X02}{1:X02}{2:X02}{3:X02}", 255, color[0], color[1], color[2]);
        }

        private static string ColorInterpolation(int[] colorMin, int[] colorMid, int[] colorMax, int minVal, int maxVal, double val)
        {

            int midVal = (minVal + maxVal) / 2;

            return ColorInterpolation(colorMin, colorMid, colorMax, minVal, midVal, maxVal, val);
        }

        private static string ColorInterpolation(int[] colorMin, int[] colorMid, int[] colorMax, int minVal, int midVal, int maxVal, double valIn)
        {
            int val = (int)valIn;

            int[] color = new int[3];

            if (val <= midVal)
            {
                maxVal = midVal;
                colorMax = colorMid;
            }
            else
            {
                minVal = midVal;
                colorMin = colorMid;
            }

            if (val > maxVal)
            {
                val = maxVal;
            }
            else if (val < minVal)
            {
                val = minVal;
            }

            for (int i = 0; i < color.Length; i++)
            {
                color[i] = (val - minVal) * (colorMax[i] - colorMin[i]) / (maxVal - minVal) + colorMin[i];
                //(x   - in_min) * (out_max   - out_min  ) / (in_max - in_min) + out_min;
            }

            return String.Format("#{0:X02}{1:X02}{2:X02}{3:X02}", 255, color[0], color[1], color[2]);
        }

        private static int LinearInterpolation(int outMin, int outMax, int minVal, int maxVal,  double val)
        {

            if (val > maxVal)
            {
                val = maxVal;
            }
            else if (val < minVal)
            {
                val = minVal;
            }
            
            return (int)Math.Round((double)((val - minVal) * (outMax - outMin) / (maxVal - minVal) + outMin));
                                         //((x   - in_min) * (out_max - out_min  ) / (in_max - in_min) + out_min;
        }

        private static double MinMax(double min, double max, double val) {

            return Math.Min(Math.Max(val, min), max);
        }

        private static string ChangeColorIfMoreThan(double val, double thisVal, int[] color, int[] colorToChange) {

            int[] colorOut = color;

            if (val > thisVal) {
                colorOut = colorToChange;
            }

            return String.Format("#{0:X02}{1:X02}{2:X02}{3:X02}", 255, colorOut[0], colorOut[1], colorOut[2]);

        }

        private string ChangeColorIfMoreThan(double val, double thisVal, int[] color, int[] colorToChange, string property) {

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
            return ChangeColorIfMoreThan(val, thisVal, color, colorToChange);

        }

        private static string ChangeColorIfLessThan(double val, double thisVal, int[] color, int[] colorToChange)
        {

            int[] colorOut = color;

            if (val < thisVal)
            {
                colorOut = colorToChange;
            }

            return String.Format("#{0:X02}{1:X02}{2:X02}{3:X02}", 255, colorOut[0], colorOut[1], colorOut[2]);

        }
        private string ChangeColorIfLessThan(double val, double thisVal, int[] color, int[] colorToChange, string property)
        {

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
            return ChangeColorIfLessThan(val, thisVal, color, colorToChange);

        }
    }
}
