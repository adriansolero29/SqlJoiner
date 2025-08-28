using Newtonsoft.Json;
using ScintillaNET;
using SqlJoiner.Interfaces.Service;
using SqlJoiner.Interfaces.View;
using SqlJoiner.Models;
using SqlJoiner.Presenter;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SqlJoiner.UI.Winforms
{
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    public partial class Form1 : Form, IJoinerMainView
    {
        #region Scintilla

        ScintillaNET.Scintilla TextArea;

        private const int BACK_COLOR = 0x2A211C;
        private const int FORE_COLOR = 0xB7B7B7;
        private const int NUMBER_MARGIN = 1;
        private const int BOOKMARK_MARGIN = 2;
        private const int BOOKMARK_MARKER = 2;
        private const int FOLDING_MARGIN = 3;
        private const bool CODEFOLDING_CIRCULAR = true;

        private void InitNumberMargin()
        {
            TextArea.Styles[Style.LineNumber].BackColor = IntToColor(BACK_COLOR);
            TextArea.Styles[Style.LineNumber].ForeColor = IntToColor(FORE_COLOR);
            TextArea.Styles[Style.IndentGuide].ForeColor = IntToColor(FORE_COLOR);
            TextArea.Styles[Style.IndentGuide].BackColor = IntToColor(BACK_COLOR);

            var nums = TextArea.Margins[NUMBER_MARGIN];
            nums.Width = 30;
            nums.Type = MarginType.Number;
            nums.Sensitive = true;
            nums.Mask = 0;

            TextArea.MarginClick += TextArea_MarginClick;
        }

        private void TextArea_MarginClick(object sender, MarginClickEventArgs e)
        {
            if (e.Margin == BOOKMARK_MARGIN)
            {
                // Do we have a marker for this line?
                const uint mask = (1 << BOOKMARK_MARKER);
                var line = TextArea.Lines[TextArea.LineFromPosition(e.Position)];
                if ((line.MarkerGet() & mask) > 0)
                {
                    // Remove existing bookmark
                    line.MarkerDelete(BOOKMARK_MARKER);
                }
                else
                {
                    // Add bookmark
                    line.MarkerAdd(BOOKMARK_MARKER);
                }
            }
        }

        private void InitSyntaxColoring()
        {

            // Configure the default style
            TextArea.StyleResetDefault();
            TextArea.Styles[Style.Default].Font = "Consolas";
            TextArea.Styles[Style.Default].Size = 10;
            TextArea.Styles[Style.Default].BackColor = IntToColor(0x212121);
            TextArea.Styles[Style.Default].ForeColor = IntToColor(0xFFFFFF);
            TextArea.StyleClearAll();

            // Configure the CPP (C#) lexer styles
            TextArea.Styles[Style.Sql.Identifier].ForeColor = IntToColor(0xD0DAE2);
            TextArea.Styles[Style.Sql.Comment].ForeColor = IntToColor(0xBD758B);
            TextArea.Styles[Style.Sql.CommentLine].ForeColor = IntToColor(0x40BF57);
            TextArea.Styles[Style.Sql.CommentDoc].ForeColor = IntToColor(0x2FAE35);
            TextArea.Styles[Style.Sql.Number].ForeColor = IntToColor(0xFFFF00);
            TextArea.Styles[Style.Sql.String].ForeColor = IntToColor(0xFFFF00);
            TextArea.Styles[Style.Sql.Character].ForeColor = IntToColor(0xE95454);
            TextArea.Styles[Style.Sql.Operator].ForeColor = IntToColor(0xE0E0E0);
            TextArea.Styles[Style.Sql.CommentLineDoc].ForeColor = IntToColor(0x77A7DB);
            TextArea.Styles[Style.Sql.Word].ForeColor = IntToColor(0x48A8EE);
            TextArea.Styles[Style.Sql.Word2].ForeColor = IntToColor(0xF98906);
            TextArea.Styles[Style.Sql.CommentDocKeyword].ForeColor = IntToColor(0xB3D991);
            TextArea.Styles[Style.Sql.CommentDocKeywordError].ForeColor = IntToColor(0xFF0000);

            TextArea.LexerName = "sql";
            //TextArea.LexerName = Lexer.SCLEX_SQL.ToString();

            TextArea.SetKeywords(0, "class extends implements import interface new case do while else if for in switch throw get set function var try catch finally while with default break continue delete return each const namespace package include use is as instanceof typeof author copy default deprecated eventType example exampleText exception haxe inheritDoc internal link mtasc mxmlc param private return see serial serialData serialField since throws usage version langversion playerversion productversion dynamic private public partial static intrinsic internal native override protected AS3 final super this arguments null Infinity NaN undefined true false abstract as base bool break by byte case catch char checked class const continue decimal default delegate do double descending explicit event extern else enum false finally fixed float for foreach from goto group if implicit in int interface internal into is lock long new null namespace object operator out override orderby params private protected public readonly ref return switch struct sbyte sealed short sizeof stackalloc static string select this throw true try typeof uint ulong unchecked unsafe ushort using var virtual volatile void while where yield");
            TextArea.SetKeywords(1, "void Null ArgumentError arguments Array Boolean Class Date DefinitionError Error EvalError Function int Math Namespace Number Object RangeError ReferenceError RegExp SecurityError String SyntaxError TypeError uint XML XMLList Boolean Byte Char DateTime Decimal Double Int16 Int32 Int64 IntPtr SByte Single UInt16 UInt32 UInt64 UIntPtr Void Path File System Windows Forms ScintillaNET");

        }

        private void InitColors()
        {
            TextArea.SetSelectionBackColor(true, IntToColor(0x114D9C));
        }

        public static Color IntToColor(int rgb)
        {
            return Color.FromArgb(255, (byte)(rgb >> 16), (byte)(rgb >> 8), (byte)rgb);
        }

        private void InitCodeFolding()
        {

            TextArea.SetFoldMarginColor(true, IntToColor(BACK_COLOR));
            TextArea.SetFoldMarginHighlightColor(true, IntToColor(BACK_COLOR));

            // Enable code folding
            TextArea.SetProperty("fold", "1");
            TextArea.SetProperty("fold.compact", "1");

            // Configure a margin to display folding symbols
            TextArea.Margins[FOLDING_MARGIN].Type = MarginType.Symbol;
            TextArea.Margins[FOLDING_MARGIN].Mask = Marker.MaskFolders;
            TextArea.Margins[FOLDING_MARGIN].Sensitive = true;
            TextArea.Margins[FOLDING_MARGIN].Width = 20;

            // Set colors for all folding markers
            for (int i = 25; i <= 31; i++)
            {
                TextArea.Markers[i].SetForeColor(IntToColor(BACK_COLOR)); // styles for [+] and [-]
                TextArea.Markers[i].SetBackColor(IntToColor(FORE_COLOR)); // styles for [+] and [-]
            }

            // Configure folding markers with respective symbols
            TextArea.Markers[Marker.Folder].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CirclePlus : MarkerSymbol.BoxPlus;
            TextArea.Markers[Marker.FolderOpen].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CircleMinus : MarkerSymbol.BoxMinus;
            TextArea.Markers[Marker.FolderEnd].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CirclePlusConnected : MarkerSymbol.BoxPlusConnected;
            TextArea.Markers[Marker.FolderMidTail].Symbol = MarkerSymbol.TCorner;
            TextArea.Markers[Marker.FolderOpenMid].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CircleMinusConnected : MarkerSymbol.BoxMinusConnected;
            TextArea.Markers[Marker.FolderSub].Symbol = MarkerSymbol.VLine;
            TextArea.Markers[Marker.FolderTail].Symbol = MarkerSymbol.LCorner;

            // Enable automatic folding
            TextArea.AutomaticFold = (AutomaticFold.Show | AutomaticFold.Click | AutomaticFold.Change);

        }

        #endregion

        public SqlJoinerMainPresenter Presenter { get; }

        public Form1(ISchemaService schemaService, ITableService tableService, IColumnService columnService, IQueryService queryService)
        {
            InitializeComponent();
            Presenter = new SqlJoinerMainPresenter(this, schemaService, tableService, columnService, queryService);

            TextArea = new ScintillaNET.Scintilla();
            txtPanel.Controls.Add(TextArea);
            TextArea.Dock = System.Windows.Forms.DockStyle.Fill;

            TextArea.WrapMode = WrapMode.None;
            TextArea.IndentationGuides = IndentView.LookBoth;

            InitColors();
            InitSyntaxColoring();
            InitNumberMargin();
            InitCodeFolding();
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<SchemaOL>? SchemaList { get; set; } = new List<SchemaOL>();
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<TableOL>? TableList { get; set; } = new List<TableOL>();
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<ColumnOL>? ColumnList { get; set; } = new List<ColumnOL>();
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<ColumnOL>? ForeignKeyColumnsList { get; set; } = new List<ColumnOL>();
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SchemaOL? CurrentSelectedSchema { get; set; } = new SchemaOL();
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TableOL? CurrentSelectedTable { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CustomRecurseColumnOL CurrentSelectedColumn { get; set; } = new CustomRecurseColumnOL();
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ColumnOL? SelectedColumn2 { get; set; } = new ColumnOL();
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string GeneratedQuery { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IEnumerable<object> GeneratedOutputFromQuery { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string JsonOutputFromQuery { get; set; }

        public event EventHandler? Init;
        public event EventHandler? SelectSchema;
        public event EventHandler? SelectTable;
        public event EventHandler? SelectColumn;
        public event EventHandler? GenerateQuery;
        public event EventHandler? LoadTableBySelectedSchema;
        public event EventHandler? LoadColumnFromSelectedTable;
        public event EventHandler? LoadForeignKeyColumnsBySelectedForeignKey;
        public event EventHandler? Reset;
        public event EventHandler? UnSelectSchema;
        public event EventHandler? UnSelectTable;
        public event EventHandler? UnSelectColumn;
        public event EventHandler? RunQuery;

        private async void Form1_Load(object sender, EventArgs e)
        {
            Init?.Invoke(sender, e);

            treeData.Nodes.Clear();
            var treeNodes = await Task.Run(() =>
            {
                var nodes = new List<TreeNode>();

                if (SchemaList == null) throw new Exception("No data provided");
                foreach (var item in SchemaList)
                {
                    var schemaNode = new TreeNode();
                    schemaNode.Text = item.SchemaName;
                    schemaNode.Tag = item;

                    nodes.Add(schemaNode);
                    schemaNode.Nodes.Add(new TreeNode());
                }

                return nodes;
            });

            treeData.Invoke(() =>
            {
                treeData.Nodes.AddRange(treeNodes.ToArray());
            });
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            GenerateQuery?.Invoke(sender, e);

            TextArea.Text = GeneratedQuery;
        }

        private async void treeData_AfterExpand(object sender, TreeViewEventArgs e)
        {
            var selectedNode = e.Node;
            if (selectedNode != null)
            {
                var selectedNodeType = selectedNode?.Tag?.GetType();
                if (selectedNodeType == typeof(SchemaOL))
                {
                    if (selectedNode != null) selectedNode.Checked = true;

                    CurrentSelectedSchema = selectedNode?.Tag as SchemaOL;
                    await Task.Run(() =>
                    {
                        LoadTableBySelectedSchema?.Invoke(sender, e);
                    });

                    if (TableList == null) return;

                    selectedNode?.Nodes?.Clear();
                    foreach (var item in TableList)
                    {
                        var tableNode = new TreeNode();
                        tableNode.Tag = item;
                        tableNode.Text = item.Name;

                        selectedNode?.Nodes.Add(tableNode);
                        tableNode.Nodes.Add(new TreeNode());
                    }
                }

                if (selectedNodeType == typeof(TableOL))
                {
                    if (selectedNode != null) selectedNode.Checked = true;
                    CurrentSelectedTable = selectedNode?.Tag as TableOL;

                    await Task.Run(() =>
                    {
                        LoadColumnFromSelectedTable?.Invoke(sender, e);
                    });

                    if (ColumnList == null) return;

                    selectedNode?.Nodes?.Clear();
                    foreach (var item in ColumnList)
                    {
                        var tableNode = new TreeNode();
                        tableNode.Tag = item;
                        tableNode.Text = item.ColumnName;

                        selectedNode?.Nodes.Add(tableNode);
                        if (item.IsForeignKey)
                            tableNode.Nodes.Add(new TreeNode());
                    }
                }

                if (selectedNodeType == typeof(ColumnOL))
                {
                    CurrentSelectedColumn.ColumnInfo = selectedNode?.Tag as ColumnOL;

                    await Task.Run(() =>
                    {
                        LoadForeignKeyColumnsBySelectedForeignKey?.Invoke(sender, e);
                    });

                    if (ForeignKeyColumnsList == null) return;

                    selectedNode?.Nodes?.Clear();
                    foreach (var item in ForeignKeyColumnsList)
                    {
                        var tableNode = new TreeNode();
                        tableNode.Tag = item;
                        tableNode.Text = item.ColumnName;

                        selectedNode?.Nodes.Add(tableNode);
                        if (item.IsForeignKey)
                            tableNode.Nodes.Add(new TreeNode());
                    }
                }
            }
        }

        private async void treeData_AfterCheck(object sender, TreeViewEventArgs e)
        {
            var selectedNode = e.Node;
            if (selectedNode != null)
            {
                var selectedNodeType = selectedNode?.Tag?.GetType();
                if (selectedNodeType == typeof(SchemaOL))
                {
                    CurrentSelectedSchema = selectedNode?.Tag as SchemaOL;
                    await Task.Run(() =>
                    {
                        if (selectedNode?.Checked == true)
                            SelectSchema?.Invoke(sender, e);
                        else
                            UnSelectSchema?.Invoke(sender, e);
                    });
                }

                if (selectedNodeType == typeof(TableOL))
                {
                    CurrentSelectedTable = selectedNode?.Tag as TableOL;
                    await Task.Run(() =>
                    {
                        if (selectedNode?.Checked == true)
                            SelectTable?.Invoke(sender, e);
                        else
                            UnSelectTable?.Invoke(sender, e);
                    });
                }

                if (selectedNodeType == typeof(ColumnOL))
                {
                    CurrentSelectedColumn.ColumnInfo = selectedNode?.Tag as ColumnOL;
                    await Task.Run(() =>
                    {
                        if (selectedNode?.Checked == true)
                            SelectColumn?.Invoke(sender, e);
                        else
                            UnSelectColumn?.Invoke(sender, e);
                    });
                }
            }
        }

        private async void btnReset_Click(object sender, EventArgs e)
        {
            Reset?.Invoke(sender, e);

            Form1_Load(sender, e);
        }

        private async void treeData_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            var selectedNode = e.Node;
            if (selectedNode != null)
            {
                selectedNode.Checked = false;
                var selectedNodeType = selectedNode?.Tag?.GetType();

                if (selectedNodeType == typeof(SchemaOL))
                {
                    CurrentSelectedSchema = selectedNode?.Tag as SchemaOL;
                    await Task.Run(() =>
                    {
                        UnSelectSchema?.Invoke(sender, e);
                    });
                }

                if (selectedNodeType == typeof(TableOL))
                {
                    CurrentSelectedTable = selectedNode?.Tag as TableOL;
                    await Task.Run(() =>
                    {
                        UnSelectTable?.Invoke(sender, e);
                    });
                }

                if (selectedNodeType == typeof(ColumnOL))
                {
                    CurrentSelectedColumn.ColumnInfo = selectedNode?.Tag as ColumnOL;
                    await Task.Run(() =>
                    {
                        SelectColumn?.Invoke(sender, e);
                    });
                }
            }
        }

        private async void btnExecute_Click(object sender, EventArgs e)
        {
            await Task.Run(() => RunQuery?.Invoke(sender, e));
            await Task.Delay(2000);
            DataTable dt2 = JsonConvert.DeserializeObject<DataTable>(JsonOutputFromQuery);
            outputDatagrid.DataSource = dt2;
        }
    }
}
