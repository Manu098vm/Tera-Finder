using System.ComponentModel;
using PKHeX.Core;

namespace TeraFinder
{
    public partial class CalculatorForm : Form
    {
        private EditorForm Editor = null!;
        private List<TeraDetails> CalculatedList = new();
        
        private TeraFilter? Filter = null;

        public CalculatorForm(EditorForm editor)
        {
            InitializeComponent();
            Editor = editor;

            var progress = (int)(Editor.Progress == GameProgress.None ? 0 : Editor.Progress);
            cmbProgress.SelectedIndex = progress;
            cmbGame.SelectedIndex = Editor.SAV.Game == (int)GameVersion.VL ? 1 : 0;
            txtTID.Text = $"{Editor.SAV.TrainerID7}";
            txtSID.Text = $"{Editor.SAV.TrainerSID7}";
            if (!IsBlankSAV()) grpGameInfo.Enabled = false;

            txtSeed.Text = Editor.txtSeed.Text;
            cmbContent.SelectedIndex = Editor.cmbContent.SelectedIndex;

            cmbSpecies.SelectedIndex = 0;
            cmbTeraType.SelectedIndex = 0;
            cmbEC.SelectedIndex = 0;
            cmbAbility.SelectedIndex = 0;
            cmbNature.SelectedIndex = 25;
            cmbGender.SelectedIndex = 0;
            cmbShiny.SelectedIndex = 0;
            nHpMax.Value = 31;
            nAtkMax.Value = 31;
            nDefMax.Value = 31;
            nSpaMax.Value = 31;
            nSpdMax.Value = 31;
            nSpeMax.Value = 31;

            dataGrid.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
            dataGrid.RowHeadersVisible = false;
            
        }

        private bool IsBlankSAV()
        {
            if(Editor.Progress is GameProgress.Beginning or GameProgress.None)
                return true;
            return false;
        }

        private void cmbContent_IndexChanged(object sender, EventArgs e)
        {
            var stars = TeraStars;
            if (cmbContent.SelectedIndex == 0)
                stars = new string[] { stars[0], stars[1], stars[2], stars[3], stars[4], stars[5] };
            if (cmbContent.SelectedIndex == 1)
                stars = new string[] { stars[6] };
            if (cmbContent.SelectedIndex == 2)
                stars = new string[] { stars[0], stars[1], stars[2], stars[3], stars[4], stars[5] };
            if (cmbContent.SelectedIndex == 3)
                stars = new string[] { stars[7] };

            cmbStars.Items.Clear();
            cmbStars.Items.AddRange(stars);
            if (cmbStars.SelectedIndex == 0)
                cmbStars_IndexChanged(sender, e);
            cmbStars.SelectedIndex = 0;
        }

        private void cmbStars_IndexChanged(object sender, EventArgs e)
        {
            var sav = !IsBlankSAV() ? Editor.SAV : new SAV9SV 
            { 
                Game = cmbGame.SelectedIndex == 0 ? (int)GameVersion.SL : (int)GameVersion.SV, TrainerID7 = Int32.Parse(txtTID.Text), TrainerSID7 = Int32.Parse(txtSID.Text) 
            };
            var species = TeraUtil.GetAvailableSpecies(sav, GetStars(), (RaidContent)cmbContent.SelectedIndex);
            cmbSpecies.Items.Clear();
            cmbSpecies.Items.Add("Any");
            cmbSpecies.Items.AddRange(species.ToArray());
            cmbSpecies.SelectedIndex = 0;
        }

        private void cmbSpecies_IndexChanged(object sender, EventArgs e)
        {
            if (cmbSpecies.SelectedIndex > 0)
            {
                var entity = GetSpeciesAndForm();
                var entry = PersonalTable.SV.GetFormEntry(entity[0], (byte)entity[1]);
                var abilites = GameInfo.Strings.abilitylist;
                cmbAbility.Items.Clear();
                cmbAbility.Items.Add("Any");
                cmbAbility.Items.Add($"{abilites[entry.Ability1]} (1)");
                cmbAbility.Items.Add($"{abilites[entry.Ability2]} (2)");
                cmbAbility.Items.Add($"{abilites[entry.AbilityH]} (H)");
            }
            else
            {
                cmbAbility.Items.Clear();
                cmbAbility.Items.Add("Any");
                cmbAbility.Items.Add("(1)");
                cmbAbility.Items.Add("(2)");
                cmbAbility.Items.Add("(H)");
            }
            cmbAbility.SelectedIndex = 0;
        }

        private void TxtSeed_KeyPress(object sender, KeyPressEventArgs e)
        {
            var c = e.KeyChar;
            if (!char.IsControl(e.KeyChar) && !((c >= '0' && c <= '9') || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F')))
                e.Handled = true;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            cmbStars.SelectedIndex = 0;
            cmbSpecies.SelectedIndex = 0;
            cmbTeraType.SelectedIndex = 0;
            cmbAbility.SelectedIndex = 0;
            cmbNature.SelectedIndex = 25;
            cmbGender.SelectedIndex = 0;
            cmbShiny.SelectedIndex = 0;
            cmbEC.SelectedIndex = 0;
            nHpMin.Value = 0;
            nAtkMin.Value = 0;
            nDefMin.Value = 0;
            nSpaMin.Value = 0;
            nSpdMin.Value = 0;
            nSpeMin.Value = 0;
            nHpMax.Value = 31;
            nAtkMax.Value = 31;
            nDefMax.Value = 31;
            nSpaMax.Value = 31;
            nSpdMax.Value = 31;
            nSpeMax.Value = 31;
        }

        private int GetStars()
        {
            if (cmbStars.Text.Equals("Any"))
                return 0;
            else
                return (int)char.GetNumericValue(cmbStars.Text[0]);
        }

        private ushort[] GetSpeciesAndForm()
        {
            var res = new ushort[2];
            if (!cmbSpecies.Text.Equals("Any"))
            {
                int charLocation = cmbSpecies.Text.IndexOf("-", StringComparison.Ordinal);

                if (charLocation == -1)
                    res[0] = (ushort)Enum.Parse(typeof(Species), cmbSpecies.Text);
                else
                {
                    res[0] = (ushort)Enum.Parse(typeof(Species), cmbSpecies.Text[..charLocation]);
                    res[1] = ShowdownParsing.GetFormFromString(cmbSpecies.Text[(charLocation + 1)..], GameInfo.Strings, res[0], EntityContext.Gen9);
                }
            }
            return res;
        }

        private Gender GetGender()
        {
            return cmbGender.SelectedIndex switch
            {
                1 => Gender.Male,
                2 => Gender.Female,
                _ => Gender.Random,
            };
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            CreateFilter();
            if (dataGrid.Rows.Count > 0)
            {
                DialogResult d = MessageBox.Show("Do you want to apply filters in the existing search?", "Apply Filters" , MessageBoxButtons.YesNo);
                if (d == DialogResult.Yes)
                {
                   
                    var list = new List<GridEntry>();
                    foreach (var c in CalculatedList)
                        if (Filter is null || Filter.IsFilterMatch(c))
                            list.Add(new GridEntry(c));
                    dataGrid.DataSource = list;
                    
                }
            }
        }

        private void CreateFilter()
        {
            var filter = new TeraFilter
            {
                MinHP = (int)nHpMin.Value,
                MaxHP = (int)nHpMax.Value,
                MinAtk = (int)nAtkMin.Value,
                MaxAtk = (int)nAtkMax.Value,
                MinDef = (int)nDefMin.Value,
                MaxDef = (int)nDefMax.Value,
                MinSpa = (int)nSpaMin.Value,
                MaxSpa = (int)nSpaMax.Value,
                MinSpd = (int)nSpdMin.Value,
                MaxSpd = (int)nSpdMax.Value,
                MinSpe = (int)nSpeMin.Value,
                MaxSpe = (int)nSpeMax.Value,
                Stars = GetStars(),
                Species = (Species)GetSpeciesAndForm()[0],
                Form = GetSpeciesAndForm()[1],
                TeraType = (MoveType)(cmbTeraType.SelectedIndex - 1),
                AbilityNumber = cmbAbility.SelectedIndex,
                Nature = (Nature)cmbNature.SelectedIndex,
                Gender = GetGender(),
                Shiny = (TeraShiny)cmbShiny.SelectedIndex,
                AltEC = cmbEC.SelectedIndex == 1,
            };

            if (Filter is null && filter.IsFilterNull())
                return;

            if (Filter is not null && Filter.CompareFilter(filter))
                return;

            Filter = filter;
        }

        private void Form_FormClosing(Object sender, FormClosingEventArgs e)
        {
            if (!token.IsCancellationRequested)
                token.Cancel();
        }
        CancellationTokenSource token = new();
     
        public async void btnSearch_Click(object sender, EventArgs e)
        {
           
            if (btnSearch.Text.Equals("Search"))
            {
               
                token = new();
                if (cmbProgress.SelectedIndex is (int)GameProgress.Beginning or (int)GameProgress.None)
                {
                    cmbProgress.Focus();
                    return;
                }
                if (txtTID.Text.Equals(""))
                {
                    txtTID.Focus();
                    return;
                }
                if (txtSID.Text.Equals(""))
                {
                    txtSID.Focus();
                    return;
                }
                try
                {
                    GetSpeciesAndForm();
                }
                catch (Exception)
                {
                    cmbSpecies.Focus();
                    return;
                }

                
                //if(dataGrid.Rows.Count> 0)
                    //dataGrid.Rows.Clear();
                if(CalculatedList.Count> 0)
                    CalculatedList.Clear();
                btnSearch.Text = "Stop";
                ActiveForm.Update();
               //grpFilters.Enabled = false;
                //grpGameInfo.Enabled = false;
                //cmbContent.Enabled = false;
                CreateFilter();
                var sav = !IsBlankSAV() ? Editor.SAV : new SAV9SV 
                { 
                    Game = cmbGame.SelectedIndex == 0 ? (int)GameVersion.SL : (int)GameVersion.SV, TrainerID7 = Int32.Parse(txtTID.Text), TrainerSID7 = Int32.Parse(txtSID.Text) 
                };
                var progress = (RaidContent)cmbContent.SelectedIndex is RaidContent.Black ? GameProgress.None : (GameProgress)cmbProgress.SelectedIndex;
                var content = (RaidContent)cmbContent.SelectedIndex;

                try
                {
                  
                   var griddata= await bgWorkerSearch_DoWork(sav, progress, content, token);
                 dataGrid.DataSource = griddata;
                    btnSearch.Text = "Search";
                }
                catch(OperationCanceledException)
                {
                    btnSearch.Text = "Search";

                }
                
            }
            else
            {
                token.Cancel();
                btnSearch.Text = "Search";
                return;
            }
        }
        private static ulong GetNext(ulong seed) { return new Xoroshiro128Plus(seed).Next(); }
        private async Task<List<GridEntry>> bgWorkerSearch_DoWork(SAV9SV sav, GameProgress progress, RaidContent content, CancellationTokenSource token)
        {
            var GridList = new List<GridEntry>();
            ulong seed = txtSeed.Text.Equals("") ? 0 : Convert.ToUInt32(txtSeed.Text, 16);
            if (seed == 0) seed = Xoroshiro128Plus.XOROSHIRO_CONST;
            var first = CalcResult(seed, progress, sav, content, 0);
            if(Filter is not null && Filter.IsFilterMatch(first))
            {
                GridList.Add(new GridEntry(first));
            }
            if (Filter is null)
                GridList.Add(new GridEntry(first));

            //var xoro = new Xoroshiro128Plus(seed);
            return await Task.Run(() =>
            {
                for (uint i = 1; i < (uint)numFrames.Value; i++)
                {

                    seed = GetNext(seed);
                    var res = CalcResult(seed, progress, sav, content, i);
                    if (Filter is not null && Filter.IsFilterMatch(res))
                    {
                        GridList.Add(new GridEntry(res));
                        CalculatedList.Add(res);
                        if (!showresults.Checked)
                            return GridList;
                    }
                    else if (Filter is null)
                    {
                        GridList.Add(new GridEntry(res));
                        CalculatedList.Add(res);
                    }
                    if (token.IsCancellationRequested)
                    {  return GridList; }
                }
                return GridList;
            }, token.Token);
        }
       
   
     


        private TeraDetails? CalcResult(ulong Seed, GameProgress progress, SAV9SV sav, RaidContent content, uint calc)
        {
            var seed = (uint)(Seed & 0xFFFFFFFF);
            var encounter = content is RaidContent.Standard or RaidContent.Black ? TeraUtil.GetTeraEncounter(seed, sav, TeraUtil.GetStars(seed, progress), Editor.Tera!) :
                content is RaidContent.Event_Mighty ? TeraUtil.GetDistEncounter(seed, sav, progress, Editor.Mighty!, true) : TeraUtil.GetDistEncounter(seed, sav, progress, Editor.Dist!, false);

            if (encounter is null)
                return null;

            return TeraUtil.CalcRNG(seed, sav.TrainerID7, sav.TrainerSID7, content, encounter, calc);
        }

        private readonly static string[] TeraStars = new string[] {
            "Any",
            "1S ☆",
            "2S ☆☆",
            "3S ☆☆☆",
            "4S ☆☆☆☆",
            "5S ☆☆☆☆☆",
            "6S ☆☆☆☆☆☆",
            "7S ☆☆☆☆☆☆☆",
        };
    }
}
