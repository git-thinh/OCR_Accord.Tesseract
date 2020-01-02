using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Reflection;
using System.Data;
using System.Text;

using Accord.Statistics.Visualizations;

using ZedGraph;
using Accord.Math;

namespace Accord.Controls
{
    public partial class HistogramView : UserControl
    {

        private Histogram m_histogram;
        private double[] m_samples;
        private BarItem m_graphBars;
        private String m_dataMember;
        private String m_displayMember;
        private object m_dataSource;
        private string m_format = "N2";


        //---------------------------------------------

        #region Constructor
        public HistogramView()
        {
            InitializeComponent();

            m_histogram = new Histogram();
            m_graphBars = new ZedGraph.BarItem(String.Empty);
            m_graphBars.Color = Color.DarkBlue;
            zedGraphControl1.GraphPane.Title.FontSpec.IsBold = true;
            zedGraphControl1.GraphPane.Title.FontSpec.Size = 32f;
            zedGraphControl1.GraphPane.Title.IsVisible = true;
            zedGraphControl1.GraphPane.XAxis.Type = AxisType.Text;
            zedGraphControl1.GraphPane.XAxis.Title.IsVisible = false;
            zedGraphControl1.GraphPane.XAxis.MinSpace = 0;
            zedGraphControl1.GraphPane.XAxis.MajorGrid.IsVisible = false;
            zedGraphControl1.GraphPane.XAxis.MinorGrid.IsVisible = false;
            zedGraphControl1.GraphPane.XAxis.MajorTic.IsBetweenLabels = true;
            zedGraphControl1.GraphPane.XAxis.MajorTic.IsInside = false;
            zedGraphControl1.GraphPane.XAxis.MajorTic.IsOpposite = false;
            zedGraphControl1.GraphPane.XAxis.MinorTic.IsAllTics = false;
            zedGraphControl1.GraphPane.XAxis.Scale.FontSpec.IsBold = true;
            zedGraphControl1.GraphPane.XAxis.Scale.FontSpec.IsAntiAlias = true;
            zedGraphControl1.GraphPane.YAxis.MinorTic.IsAllTics = false;
            zedGraphControl1.GraphPane.YAxis.MajorTic.IsOpposite = false;
            zedGraphControl1.GraphPane.YAxis.Title.Text = "Frequency";
            zedGraphControl1.GraphPane.YAxis.Title.FontSpec.Size = 24f;
            zedGraphControl1.GraphPane.YAxis.Title.FontSpec.IsBold = true;
            zedGraphControl1.GraphPane.Border.IsVisible = false;
            zedGraphControl1.GraphPane.BarSettings.MinBarGap = 0;
            zedGraphControl1.GraphPane.BarSettings.MinClusterGap = 0;
            zedGraphControl1.GraphPane.CurveList.Add(m_graphBars);
        }
        #endregion

        //---------------------------------------------

        #region Properties
        public ZedGraphControl Graph
        {
            get { return zedGraphControl1; }
        }

        public TrackBar TrackBar
        {
            get { return trackBar1; }
        }

        public Histogram Histogram
        {
            get { return m_histogram; }
        }

        [DefaultValue(null)]
        public object DataSource
        {
            get { return m_dataSource; }
            set
            {
                m_dataSource = value;
                if (!this.DesignMode)
                OnDataBind();
            }
        }

        [DefaultValue(null)]
        public string DataMember
        {
            get { return m_dataMember; }
            set
            {
                m_dataMember = value;
                if (!this.DesignMode)
                OnDataBind();
            }
        }

        [DefaultValue(null)]
        public string DisplayMember
        {
            get { return m_displayMember; }
            set
            {
                m_displayMember = value;

                if (!this.DesignMode)
                    OnDataBind();
            }
        }

        [DefaultValue("N2")]
        public string Format
        {
            get { return m_format; }
            set
            {
                m_format = value;
                if (!this.DesignMode)
                    OnDataBind();
            }
        }
        #endregion

        //---------------------------------------------

        #region Public Members
        public void UpdateGraph()
        {
            m_graphBars.Clear();


            String[] labels = new String[m_histogram.Values.Length];
            for (int i = 0; i < m_histogram.Values.Length; i++)
			{
                m_graphBars.AddPoint(i, m_histogram.Bins[i].Value);
                labels[i] = m_histogram.Bins[i].Range.Min.ToString(m_format) +
                    " - " + m_histogram.Bins[i].Range.Max.ToString(m_format);
			}

            zedGraphControl1.GraphPane.XAxis.Scale.TextLabels = labels;
            zedGraphControl1.GraphPane.XAxis.Scale.FontSpec.Angle = 45.0f;

            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
        }

        private void UpdateTrackbar()
        {
            trackBar1.Maximum = m_samples.Length;
                /*(int)RangeConversion.Convert(
                                        m_histogram.Range.Length,
                                        new DoubleRange(0, m_histogram.Range.Length),
                                        new DoubleRange(0, 100)
                                    );*/
        }
        #endregion

        //---------------------------------------------

        #region Private Members
        private void OnDataBind()
        {
            if (m_dataSource == null)
                return;

            if (m_histogram == null)
                m_histogram = new Histogram();

            if (m_dataSource is DataSet)
            {
                if (m_dataMember != null && m_dataMember.Length > 0)
                {
                    if (m_displayMember != null && m_displayMember.Length > 0)
                    {
                        //m_samples = new SampleVector(((DataSet)m_dataSource).Tables[m_dataMember].Columns[m_displayMember]);
                    }
                    else
                    {
                      //  m_samples = new SampleVector(((DataSet)m_dataSource).Tables[m_dataMember].Columns[0]);
                    }
                }
                else
                {
                    //m_samples = new SampleVector(((DataSet)m_dataSource).Tables[0].Columns[0]);
                }
            }
            else if (m_dataSource is DataTable)
            {
                DataTable table = m_dataSource as DataTable;

                if (m_dataMember != null && m_dataMember.Length > 0)
                {
                    //m_samples = new SampleVector(((DataTable)m_dataSource).Columns[m_displayMember]);
                    
                    DataColumn column = table.Columns[m_dataMember];
                    m_samples = Matrix.ToArray(column);
                    
                }
                else
                {
                    return;
                    //m_samples = new SampleVector(((DataTable)m_dataSource).Columns[0]);
                }
            }
            else if (m_dataSource is double[])
            {
                m_samples = m_dataSource as double[] ;
            }
            else if (m_dataSource is IListSource)
            {
                //m_samples = new SampleVector((IListSource)m_dataSource);
            }
            else
            {
                return;
                // invalid data source
            }

            zedGraphControl1.GraphPane.Title.Text = m_histogram.Title;
            this.m_histogram.Compute(m_samples);
            
            this.UpdateTrackbar();
            this.UpdateGraph();

            this.trackBar1.Value = m_histogram.Count;
            
        }
        #endregion

        //---------------------------------------------

        #region Event Handling
        private void trackBar_ValueChanged(object sender, EventArgs e)
        {
            m_histogram.Compute(m_samples, (int)trackBar1.Value);
            /* m_histogram.Compute(m_samples, RangeConversion.Convert(
                                        trackBar1.Value,
                                        new DoubleRange(0, 100),
                                        new DoubleRange(0, m_histogram.Range.Length)
                                   ));
             */
            this.UpdateGraph();
        }
        #endregion

    }
}
