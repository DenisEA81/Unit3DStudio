using System;
using System.Drawing;
using Graph3DLibrary;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using System.Threading.Tasks;


namespace Unit3DStudio
{
    public partial class frmMain : Form
    {
        #region Переменные для автосохранения
        public string TempDirectory = "\\Temp";
        public string TempProjectFileName = "\\AutoSaveChanges";
        public string ScreenshotFolder = "\\Screenshots";
        public static string ApplicationFolder = "";
        public static string iniUserSettings = "\\UserSettings.ini";
        public string TempProjectName = "";
        public int MaxAutoSaveBufferSize = 250;
        public int CurrentAutoSaveCount = 0;
        public int CurrentAutoSaveIndex = 0;
        public int RedoAutoSaveIndex = 0;
        public bool isEditedProject = false;
        public bool isEditedObject = false;
        public ProjectSaver projectSaver = new ProjectSaver();
        #endregion

        #region Скриншоты
        public static bool ScreenshotMode = false;
        public static bool ScreenshotAlpha = true;
        public static bool ScreenshotFrame = false;
        public static bool ScreenshotSeries = false;
        public static float ScreenshotSeriesStep = Engine3D.Radian90 / 2;
        public static float ScreenshotSeriesPos = 0;
        public static float ScreenshotSeriesAngle = 0;
        public static float ScreenshotSeriesCameraZ = 0;
        public static Point ScreenshotSize = new Point(512, 512);
        public static string ScreenshotFileName = "";
        public static int ScreenshotSeriesFileNum = 0;
        public static int ScreenshotSeriesFileMask = 3;
        public static Point WindowFrame = new Point(512, 512);
        public static bool WindowInFrame = false;
        #endregion

        #region Настройки курсора мыши
        public static float MouseXYDiv = 100f;
        public static float MouseWheelDiv = 20f;
        public static float MouseAxisDiv = 100f;
        public static bool MouseWheelInverse = false;
        #endregion

        /// <summary>
        /// Отключение событий визуальных элементов
        /// </summary>
        public int VisualComponentMethodLock = 0;


        #region Поверхности рисования и их настройки
        IDrawingSurface Surface = null;
        IDrawingSurfaceBuilder SurfaceBuilder = null;
        int SurfaceBufferCount = 0;
        #endregion

        #region Коэффициент перспективы
        public const float PerspectiveK = 1000;
        #endregion

        #region Камера
        //public float CameraAngleXY = ;
        //public float CameraAngleYZ = ;
        //public float CameraAngleZX = ;
        public Point3D CameraAngle = new Point3D(Engine3D.RadianDegrees * 0, Engine3D.RadianDegrees * 30, Engine3D.RadianDegrees * 225);



        // Начальные координаты камеры
        public const float CameraPosX = 0;
        public const float CameraPosY = 0;
        public const float CameraPosZ = -700;

        public Point3D CameraPos;  // Вектор положения камеры

        public bool MovingCamera = false;
        public int MouseRightX = 0;
        public int MouseRightY = 0;

        public bool RollingCamera = false;
        public int MouseLeftX = 0;
        public int MouseLeftY = 0;

        public bool BankCamera = false;
        public int MouseCenterX = 0;
        public int MouseCenterY = 0;
        #endregion

        #region Модели
        public int ModelDrawType = 2;        // Тип отрисовки полигонов (0-паутина (без заливки), 1- полигоны (с заливкой), 2 - автовыбор)    
        public const int MaxUnitCount = 1000;   // Максимальное количество моделей-примитивов
        public int UnitCount;   // текущий размер буфера моделей
        public VolumetricModel3D[] model;    // Модели
        public int[] ActiveUnitIndex;        // Список-стек активных моделей
        public int ActiveUnitIndexCount;     // Количество активных моделей
        public MeshModifications[] MeshMod = new MeshModifications[MaxUnitCount];   // модицикации моделей
        public const int MaxModifyCount = 50; // максимальное количество модификаций на модель
        #endregion

        #region Координатные оси
        public const int CoordLineCount = 3; // Количество координатных линий
        public const int sectionCount = 25;
        public const int sectionHeight = 8;
        public const int sectionR = 3;
        public Point3D[] CoordLineTopMain; // координаты для вывода наименования осей
        public Point3D[] CoordLineTopCamera; // координаты для вывода наименования осей                
        public VolumetricModel3D[] ModelAxis;    // ссылки на оси
        #endregion

        #region Метки выбора объекта
        public Point MouseSelectPos = new Point(-1, -1); // позиция выбора
        public int MouseSelectedObjectIdx = -2; // обозначенный для выбора
        public int SelectedObjectIdx = -1;      // выбранный  
        public const int SelectionLineCount = 6; // Количество линий обозначения выбора
        public const int SelectionSectionCount = 6;
        public const int SelectionSectionHeight = 4;
        public const int SelectionSectionR = 2;
        public VolumetricModel3D[] ModelSelection;    // ссылки на оси
        public Point3D SelectionObjectPosition = new Point3D(); // центр выбранного объекта
        #endregion

        #region Контроллер коллекции моделей
        ModelCollectionController modelController;
        #endregion

        #region Цвет фона
        public int BackgroundColorR = 50;
        public int BackgroundColorG = 100;
        public int BackgroundColorB = 100;
        #endregion

        #region Освещение

        #region Фоновое освещение
        public int AmbientColorR = 0;
        public int AmbientColorG = 0;
        public int AmbientColorB = 0;
        public float AmbientLightPower = (float)2;
        #endregion

        #region параллельные источники света
        // сила света
        public float[] DirectLightPower = new float[] { (float)0.65 };

        // цвет 
        public Color[] DirectLightColor = new Color[] { Color.FromArgb(255, 255, 255, 255) };

        // Начальные векторы света
        public int[] DirectLightVectorX = new int[] { 0 };
        public int[] DirectLightVectorY = new int[] { -1 };
        public int[] DirectLightVectorZ = new int[] { 2 };
        public Point3D[] DirectVectorLight = new Point3D[1];
        #endregion

        #endregion

        public frmMain()
        {
            InitializeComponent();
        }

        public static void ErrorProtocol(string procName, string errorMessage)
        {
            MessageBox.Show(errorMessage, "Ошибка в модуле: " + procName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        private void закрытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void новыйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewProject();
        }

        public DialogResult NewProject(bool isRecovery = false)
        {
            try
            {
                if (!isRecovery)
                {
                    timerDraw.Enabled = false;
                    if (isEditedProject & (UnitCount > CoordLineCount + SelectionLineCount))
                    {
                        switch (MessageBox.Show("Сохранить текущий проект?", "Внимание!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning))
                        {
                            case DialogResult.Yes:
                                сохранитьПроектКакToolStripMenuItem_Click(null, null);
                                break;
                            case DialogResult.Cancel:
                                {
                                    timerDraw.Enabled = true;
                                    return DialogResult.Cancel;
                                }
                        }
                    }

                    this.Text = "Студия трехмерных моделей - новый проект";
                }
                openProjectDlg.FileName = "";
                saveProjectDlg.FileName = "";

                for (int i = CoordLineCount + SelectionLineCount; i < UnitCount; i++)
                {
                    model[i] = null;
                    MeshMod[i - CoordLineCount - SelectionLineCount] = null;
                }
                UnitCount = CoordLineCount + SelectionLineCount;
                MeshListUpdate();

                SelectedObjectIdx = -1;
                DeselectObject();

                #region Инициализация буфера сортировки моделей
                ActiveUnitIndex = new int[UnitCount];
                #endregion

                #region Инициализация контроллера моделей
                for (int i = 0; i < modelController.Collection.Length; i++) modelController.Collection[i] = null;
                for (int i = 0; i < UnitCount; i++)
                    if (model[i] != null) modelController.Collection[i] = model[i];
                if (modelController.CreateActivePolygonBuffer() != 0) throw new Exception(ErrorLog.GetLastError());
                #endregion

                if (!isRecovery)
                {
                    CurrentAutoSaveCount = 0;
                    CurrentAutoSaveIndex = 0;
                    RedoAutoSaveIndex = 0;
                    isEditedProject = false;
                    isEditedObject = false;
                    toolStripButton3.Enabled = false;
                    toolStripButton10.Enabled = false;
                    timerDraw.Enabled = true;
                }
                return DialogResult.OK;
            }
            catch (Exception er)
            {
                ErrorProtocol("новыйToolStripMenuItem_Click", er.Message);
                timerDraw.Enabled = true;
                return DialogResult.None;
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            try
            {
                #region Инициализация интерфейса
                VisualComponentMethodLock = 0;

                #region Каталог временных файлов
                try
                {
                    string tmpS = Application.ExecutablePath;
                    TempDirectory = tmpS.Substring(0, tmpS.LastIndexOf("\\")) + TempDirectory + DateTime.Now.Ticks.ToString();
                    ScreenshotFolder = tmpS.Substring(0, tmpS.LastIndexOf("\\")) + ScreenshotFolder;
                    ApplicationFolder = tmpS.Substring(0, tmpS.LastIndexOf("\\"));
                }
                catch
                {
                    TempDirectory = "c:" + TempDirectory;
                    ScreenshotFolder = "c:" + ScreenshotFolder;
                    ApplicationFolder = "c:";
                }

                #region Чтение пользовательских настроек
                ReadUserSettings();
                #endregion

                try
                {
                    if (!Directory.Exists(TempDirectory)) Directory.CreateDirectory(TempDirectory);

                }
                catch (Exception er)
                {
                    MessageBox.Show("Ошибка при создании каталога для хранения временных файлов.\n" + er.Message);
                }
                TempProjectName = TempDirectory + TempProjectFileName;

                #endregion

                isEditedProject = false;
                isEditedObject = false;

                timerAutoSaveChanges.Enabled = true;

                #region Для идентификации переключателей
                rbCenter.Tag = 0;
                rbXZ.Tag = 1;
                rbYZ.Tag = 2;
                rbXY.Tag = 3;
                #endregion

                #endregion

                #region Задаем начальное положение камеры
                CameraPos = new Point3D(-CameraPosX, -CameraPosY, -CameraPosZ);
                #endregion

                #region Инициализация источников освещения
                for (int i = 0; i < DirectVectorLight.Length; i++)
                {
                    DirectVectorLight[i] = new Point3D(DirectLightVectorX[i], DirectLightVectorY[i], DirectLightVectorZ[i]);
                    Engine3D.NormalizeVector(DirectVectorLight[i]);
                }
                #endregion

                #region Инициализация моделей
                UnitCount = CoordLineCount + SelectionLineCount;

                #region Основной буфер моделей
                model = new VolumetricModel3D[MaxUnitCount];
                #endregion

                #region Система координат
                {
                    CoordLineTopMain = new Point3D[CoordLineCount];
                    CoordLineTopCamera = new Point3D[CoordLineCount];

                    ModelAxis = new VolumetricModel3D[CoordLineCount];
                    for (int i = 0; i < CoordLineCount; i++)
                    {
                        ModelAxis[i] = model[i] = new Poly3D(13, sectionCount, 0, Engine3D.Radian360, sectionR, 0);
                        model[i].CreatePolygonMap();
                        model[i].SetDoubleSided(true);

                        switch (i)
                        {
                            case 0:
                                model[i].SetColor(Color.FromArgb(100, Color.Purple.R, Color.Purple.G, Color.Purple.B));
                                break;
                            case 1:
                                model[i].SetColor(Color.FromArgb(100, Color.BlueViolet.R, Color.BlueViolet.G, Color.BlueViolet.B));
                                break;
                            case 2:
                                model[i].SetColor(Color.FromArgb(100, Color.GreenYellow.R, Color.GreenYellow.G, Color.GreenYellow.B));
                                break;
                            default:
                                model[i].SetColor(Color.FromArgb(100, 0, 0, 0));
                                break;
                        }
                    }

                    for (int i = 0; i < CoordLineCount; i++)
                    {
                        ((Poly3D)model[i]).MoveSectionTo(0, 0, 0, sectionHeight);
                        ((Poly3D)model[i]).SectionParameter[0].PositiveRadius.X = 0;
                        ((Poly3D)model[i]).SectionParameter[0].PositiveRadius.Y = 0;
                        ((Poly3D)model[i]).SectionParameter[0].NegativeRadius.X = 0;
                        ((Poly3D)model[i]).SectionParameter[0].NegativeRadius.Y = 0;
                        ((Poly3D)model[i]).CalculatePoly3DSection(0);

                        ((Poly3D)model[i]).MoveSectionTo(sectionCount - 2, 0, 0, (sectionCount - 3) * sectionHeight);
                        ((Poly3D)model[i]).SectionParameter[sectionCount - 2].PositiveRadius.X = sectionR * 2;
                        ((Poly3D)model[i]).SectionParameter[sectionCount - 2].PositiveRadius.Y = sectionR * 2;
                        ((Poly3D)model[i]).SectionParameter[sectionCount - 2].NegativeRadius.X = sectionR * 2;
                        ((Poly3D)model[i]).SectionParameter[sectionCount - 2].NegativeRadius.Y = sectionR * 2;
                        ((Poly3D)model[i]).CalculatePoly3DSection(sectionCount - 2);

                        ((Poly3D)model[i]).MoveSectionTo(sectionCount - 1, 0, 0, (sectionCount - 1) * sectionHeight);
                        ((Poly3D)model[i]).SectionParameter[sectionCount - 1].PositiveRadius.X = 0;
                        ((Poly3D)model[i]).SectionParameter[sectionCount - 1].PositiveRadius.Y = 0;
                        ((Poly3D)model[i]).SectionParameter[sectionCount - 1].NegativeRadius.X = 0;
                        ((Poly3D)model[i]).SectionParameter[sectionCount - 1].NegativeRadius.Y = 0;
                        ((Poly3D)model[i]).CalculatePoly3DSection(sectionCount - 1);

                        CoordLineTopMain[i] = new Point3D(0, 0, sectionCount * sectionHeight);

                        for (int j = 1; j < sectionCount - 2; j++)
                        {
                            ((Poly3D)model[i]).MoveSectionTo(j, 0, 0, j * sectionHeight);
                            ((Poly3D)model[i]).CalculatePoly3DSection(j);
                        }
                        model[i].ResetCameraModel();


                        switch (i)
                        {
                            case 0:
                                model[i].Rotate(-Engine3D.Radian90, Axis3D.OYxz);
                                Engine3D.RotatePoint3D(-Engine3D.Radian90, Axis3D.OYxz, CoordLineTopMain[i]);
                                break;
                            case 1:
                                model[i].Rotate(Engine3D.Radian90, Axis3D.OXyz);
                                Engine3D.RotatePoint3D(Engine3D.Radian90, Axis3D.OXyz, CoordLineTopMain[i]);
                                break;
                                /*
                            case 2:
                                model[i].Rotate(0, Axis3D.OXyz);
                                break;
                                 */
                        }

                        CoordLineTopCamera[i] = new Point3D(CoordLineTopMain[i]);

                        model[i].SaveCameraModelToMain();
                    }


                }
                #endregion

                #region Метки выбора объекта
                ModelSelection = new VolumetricModel3D[SelectionLineCount];
                for (int i = CoordLineCount; i < CoordLineCount + SelectionLineCount; i++)
                {
                    ModelSelection[i - CoordLineCount] = model[i] = new Poly3D(7, SelectionSectionCount, 0, Engine3D.Radian360, SelectionSectionR, 0);
                    model[i].CreatePolygonMap();
                    model[i].SetDoubleSided(true);
                    model[i].SetColor(Color.FromArgb(0, Color.Red.R, Color.Red.G, Color.Red.B));
                }

                for (int i = CoordLineCount; i < CoordLineCount + SelectionLineCount; i++)
                {
                    ((Poly3D)model[i]).MoveSectionTo(0, 0, 0, SelectionSectionHeight);
                    ((Poly3D)model[i]).SectionParameter[0].PositiveRadius.X = 0;
                    ((Poly3D)model[i]).SectionParameter[0].PositiveRadius.Y = 0;
                    ((Poly3D)model[i]).SectionParameter[0].NegativeRadius.X = 0;
                    ((Poly3D)model[i]).SectionParameter[0].NegativeRadius.Y = 0;
                    ((Poly3D)model[i]).CalculatePoly3DSection(0);

                    ((Poly3D)model[i]).MoveSectionTo(SelectionSectionCount - 2, 0, 0, (SelectionSectionCount - 3) * SelectionSectionHeight);
                    ((Poly3D)model[i]).SectionParameter[SelectionSectionCount - 2].PositiveRadius.X = SelectionSectionR * 2;
                    ((Poly3D)model[i]).SectionParameter[SelectionSectionCount - 2].PositiveRadius.Y = SelectionSectionR * 2;
                    ((Poly3D)model[i]).SectionParameter[SelectionSectionCount - 2].NegativeRadius.X = SelectionSectionR * 2;
                    ((Poly3D)model[i]).SectionParameter[SelectionSectionCount - 2].NegativeRadius.Y = SelectionSectionR * 2;
                    ((Poly3D)model[i]).CalculatePoly3DSection(SelectionSectionCount - 2);

                    ((Poly3D)model[i]).MoveSectionTo(SelectionSectionCount - 1, 0, 0, (SelectionSectionCount - 1) * SelectionSectionHeight);
                    ((Poly3D)model[i]).SectionParameter[SelectionSectionCount - 1].PositiveRadius.X = 0;
                    ((Poly3D)model[i]).SectionParameter[SelectionSectionCount - 1].PositiveRadius.Y = 0;
                    ((Poly3D)model[i]).SectionParameter[SelectionSectionCount - 1].NegativeRadius.X = 0;
                    ((Poly3D)model[i]).SectionParameter[SelectionSectionCount - 1].NegativeRadius.Y = 0;
                    ((Poly3D)model[i]).CalculatePoly3DSection(SelectionSectionCount - 1);

                    for (int j = 1; j < SelectionSectionCount - 2; j++)
                    {
                        ((Poly3D)model[i]).MoveSectionTo(j, 0, 0, j * SelectionSectionHeight);
                        ((Poly3D)model[i]).CalculatePoly3DSection(j);
                    }
                    model[i].ResetCameraModel();

                    model[i].Move(0, 0, -SelectionSectionHeight * SelectionSectionCount);
                    switch (i)
                    {
                        case 3:
                            model[i].Rotate(-Engine3D.Radian90, Axis3D.OYxz);
                            break;
                        case 4:
                            model[i].Rotate(Engine3D.Radian90, Axis3D.OXyz);
                            break;
                        case 5:
                            model[i].Rotate(0, Axis3D.OXyz);
                            break;
                        case 6:
                            model[i].Rotate(Engine3D.Radian90, Axis3D.OYxz);
                            break;
                        case 7:
                            model[i].Rotate(-Engine3D.Radian90, Axis3D.OXyz);
                            break;
                        case 8:
                            model[i].Rotate(Engine3D.Radian180, Axis3D.OXyz);
                            break;
                    }
                    model[i].SaveCameraModelToMain();
                }
                #endregion

                #endregion

                #region Инициализация буфера сортировки моделей
                ActiveUnitIndex = new int[UnitCount];
                #endregion

                #region Инициализация контроллера моделей
                modelController = new ModelCollectionController(MaxUnitCount);
                for (int i = 0; i < UnitCount; i++) if (model[i] != null) modelController.Collection[i] = model[i];
                if (modelController.CreateActivePolygonBuffer() != 0) throw new Exception(ErrorLog.GetLastError());
                #endregion

                SurfaceBufferCount = 3;// (Environment.ProcessorCount == 1) ? 1 : (Environment.ProcessorCount / 2);

                SurfaceBuilder = new DrawingSurfaceBuilder(pictMain, SurfaceBufferCount);

                this.WindowState = FormWindowState.Maximized;

                timerDraw.Enabled = true;
            }
            catch (Exception er)
            {
                ErrorProtocol("frmMain_Load", er.Message);
            }
        }

        private void ReadUserSettings()
        {
            try
            {
                MouseWheelInverse = false;
                if (!File.Exists(ApplicationFolder + iniUserSettings)) return;
                StreamReader ini = new StreamReader(ApplicationFolder + iniUserSettings);
                try
                {
                    while (!ini.EndOfStream)
                    {
                        string[] line = ini.ReadLine().Split('=');
                        if (line.Length != 2) continue;

                        switch (line[0].ToUpper().Trim())
                        {
                            case "MOUSEWHEELINVERTING":
                                MouseWheelInverse = line[1] == "1";
                                break;

                        }
                    }
                }
                finally
                {
                    ini.Close();
                }
            }
            catch (Exception er)
            {
                ErrorProtocol("ReadUserSettings", er.Message);
            }
        }

        public static void SaveUserSettings()
        {
            try
            {
                if (File.Exists(ApplicationFolder + iniUserSettings)) File.Delete(ApplicationFolder + iniUserSettings);
                Application.DoEvents();
                StreamWriter ini = new StreamWriter(ApplicationFolder + iniUserSettings);
                try
                {
                    ini.WriteLine("[User Settings File]");
                    ini.WriteLine("MouseWheelInverting=" + (MouseWheelInverse ? "1" : "0"));
                }
                finally
                {
                    ini.Close();
                }
            }
            catch (Exception er)
            {
                ErrorProtocol("SaveUserSettings", er.Message);
            }
        }


        private void timerDraw_Tick(object sender, EventArgs e)
        {
            try
            {
                timerDraw.Enabled = false;
                if (pictMain.ClientRectangle.Width == 0) return;

                FPSStatistics fpsStatistics = new FPSStatistics("Старт", 100);

                #region Инициализируем поверхности рисования при необходимости
                if (Surface == null) Surface = SurfaceBuilder.Instance();
                else Surface.ResetSurfaces();
                #endregion

                #region Создание и отрисовка сцены 

                #region Заливаем поверхность рисования цветом фона 
                Surface.ClearSurfaces(Color.FromArgb((ScreenshotMode & ScreenshotAlpha) ? 0 : 255, BackgroundColorR, BackgroundColorG, BackgroundColorB));
                #endregion заливаем поверхность рисования цветом фона                

                if (ScreenshotMode)
                {
                    SelectedObjectIdx = -1;
                    DeselectObject();
                }
                fpsStatistics.NextPoint("Инициализация и залифка фона");

                #region Сбрасываем список активных моделей (юнитов)                
                Parallel.For(0, UnitCount, (int i) => { ActiveUnitIndex[i] = i; });
                ActiveUnitIndexCount = UnitCount;
                fpsStatistics.NextPoint("Сбрасываем список активных моделей (юнитов)");
                #endregion

                #region Сбрасываем индексы полигонов акитивных моделей                
                Parallel.For(0, ActiveUnitIndexCount, (int i) =>
                  {
                      if (model[ActiveUnitIndex[i]] != null)
                          if (model[ActiveUnitIndex[i]].ResetActivePolygonIndexes() != 0) throw new Exception(Graph3DLibrary.ErrorLog.GetLastError());
                  });
                fpsStatistics.NextPoint("Сбрасываем индексы полигонов акитивных моделей");
                #endregion сбрасываем индексы полигонов акитивных моделей

                #region Сброс вершин деформаций                
                Parallel.For(0, ActiveUnitIndexCount, (int i) =>
                  {
                      if (model[ActiveUnitIndex[i]] != null)
                          if (model[ActiveUnitIndex[i]].ResetCameraModel() != 0) throw new Exception(Graph3DLibrary.ErrorLog.GetLastError());
                  });
                fpsStatistics.NextPoint("Сброс вершин деформаций");
                #endregion

                #region Обработка модификаторов                
                Parallel.For(0, ActiveUnitIndexCount, (int i) =>
                  {
                      if (ActiveUnitIndex[i] < CoordLineCount + SelectionLineCount) return;
                      ModifyModel(model[ActiveUnitIndex[i]], MeshMod[ActiveUnitIndex[i] - CoordLineCount - SelectionLineCount]);
                  });
                MouseSelectedObjectIdx = SelectedObjectIdx;
                MouseSelectObject(true);
                fpsStatistics.NextPoint("Обработка модификаторов");
                #endregion

                #region Поворот камеры

                #region Освещение                
                for (int i = 0; i < DirectVectorLight.Length; i++)
                {
                    DirectVectorLight[i].CopyFrom(DirectLightVectorX[i], DirectLightVectorY[i], DirectLightVectorZ[i]);
                    if (Engine3D.NormalizeVector(DirectVectorLight[i]) != 0) throw new Exception(Graph3DLibrary.ErrorLog.GetLastError());
                    //if (Engine3D.RotatePoint3D(CameraAngleZX, Axis3D.OYxz, DirectVectorLight[i]) != 0) throw new Exception(Graph3DLibrary.ErrorLog.GetLastError());
                    //if (Engine3D.RotatePoint3D(CameraAngleYZ, Axis3D.OXyz, DirectVectorLight[i]) != 0) throw new Exception(Graph3DLibrary.ErrorLog.GetLastError());
                }
                fpsStatistics.NextPoint("Поворот камеры - Освещение");
                #endregion

                #region Подписи к линиям координат                
                for (int i = 0; i < 3; i++)
                {
                    CoordLineTopCamera[i].CopyFrom(CoordLineTopMain[i]);
                    if (Engine3D.RotatePoint3D(CameraAngle.X, Axis3D.OXyz, CoordLineTopCamera[i]) != 0) throw new Exception(Graph3DLibrary.ErrorLog.GetLastError());
                    if (Engine3D.RotatePoint3D(CameraAngle.Z, Axis3D.OYxz, CoordLineTopCamera[i]) != 0) throw new Exception(Graph3DLibrary.ErrorLog.GetLastError());
                    if (Engine3D.RotatePoint3D(CameraAngle.Y, Axis3D.OXyz, CoordLineTopCamera[i]) != 0) throw new Exception(Graph3DLibrary.ErrorLog.GetLastError());
                    if (Engine3D.MovePoint3D(CameraPos.X, CameraPos.Y, CameraPos.Z, CoordLineTopCamera[i]) != 0) throw new Exception(Graph3DLibrary.ErrorLog.GetLastError());
                }
                fpsStatistics.NextPoint("Поворот камеры - Подписи к линиям координат");
                #endregion

                #region Модели                
                Parallel.For(0, ActiveUnitIndexCount, (int i) =>
                  {
                      if (model[ActiveUnitIndex[i]] != null)
                      {
                          if (ScreenshotMode & ScreenshotSeries)
                              if (model[ActiveUnitIndex[i]].Rotate(ScreenshotSeriesPos, Axis3D.OYxz) != 0) throw new Exception(Graph3DLibrary.ErrorLog.GetLastError());

                          if (model[ActiveUnitIndex[i]].Rotate(CameraAngle.X, Axis3D.OXyz) != 0) throw new Exception(Graph3DLibrary.ErrorLog.GetLastError());
                          if (model[ActiveUnitIndex[i]].Rotate(CameraAngle.Z, Axis3D.OYxz) != 0) throw new Exception(Graph3DLibrary.ErrorLog.GetLastError());
                          if (model[ActiveUnitIndex[i]].Rotate(CameraAngle.Y, Axis3D.OXyz) != 0) throw new Exception(Graph3DLibrary.ErrorLog.GetLastError());

                          if (ScreenshotMode & ScreenshotSeries)
                              if (model[ActiveUnitIndex[i]].Rotate(ScreenshotSeriesAngle, Axis3D.OXyz) != 0) throw new Exception(Graph3DLibrary.ErrorLog.GetLastError());

                          if (model[ActiveUnitIndex[i]].Move(CameraPos.X, CameraPos.Y, (ScreenshotMode & ScreenshotSeries) ? ScreenshotSeriesCameraZ : CameraPos.Z) != 0) throw new Exception(Graph3DLibrary.ErrorLog.GetLastError());
                      }
                  });
                if (ScreenshotMode & ScreenshotSeries)
                    ScreenshotSeriesPos += ScreenshotSeriesStep;
                fpsStatistics.NextPoint("Поворот камеры - Модели");
                #endregion модели

                #endregion поворот камеры

                #region Обработка полигонов

                #region Создаем экранное пространство                
                Parallel.For(0, ActiveUnitIndexCount, (int i) =>
                     {
                         if (model[ActiveUnitIndex[i]] != null)
                             if (model[ActiveUnitIndex[i]].CreateScreenVertex(PerspectiveK, (int)(pictMain.ClientRectangle.Width / 2), (int)(-pictMain.ClientRectangle.Height / 2)) != 0) throw new Exception(Graph3DLibrary.ErrorLog.GetLastError());
                     });

                if (Engine3D.Point3DPresentationConversion(CoordLineTopCamera, PerspectiveK, (int)(pictMain.ClientRectangle.Width / 2), (int)(-pictMain.ClientRectangle.Height / 2)) != 0) throw new Exception(Graph3DLibrary.ErrorLog.GetLastError());
                fpsStatistics.NextPoint("Создание экранного пространства");
                #endregion

                #region Расчет центров полигонов для расчета освещения                
                Parallel.For(0, ActiveUnitIndexCount, (int i) =>
                  {
                      if (model[ActiveUnitIndex[i]] != null)
                          if (model[ActiveUnitIndex[i]].CalculatePolygonCenters() != 0) throw new Exception(Graph3DLibrary.ErrorLog.GetLastError());
                  });
                fpsStatistics.NextPoint("Расчет центров полигонов для расчета освещения");
                #endregion

                #region Фильтруем невидимые полигоны (часть 1)
                {
                    Point tmpPoint = new Point(pictMain.ClientRectangle.Width, pictMain.ClientRectangle.Height);
                    Parallel.For(0, ActiveUnitIndexCount, (int i) =>
                      {
                          if (model[ActiveUnitIndex[i]] != null)
                          {
                              #region Фильтрация слишком близко расположенных полигонов
                              if (model[ActiveUnitIndex[i]].FilterPolygonByZPos(0, -1) != 0) throw new Exception(Graph3DLibrary.ErrorLog.GetLastError());
                              #endregion

                              #region Фильтрация полигонов, расположенных за пределами экрана
                              if (model[ActiveUnitIndex[i]].FilterPolygonByXYPos(tmpPoint) != 0) throw new Exception(Graph3DLibrary.ErrorLog.GetLastError());
                              #endregion
                          }
                      });
                    fpsStatistics.NextPoint("Фильтруем невидимые полигоны (часть 1)");
                }
                #endregion

                #region Расчет нормалей к полигонам                
                Parallel.For(0, ActiveUnitIndexCount, (int i) =>
                 {
                     if (model[ActiveUnitIndex[i]] != null)
                         if (model[ActiveUnitIndex[i]].ActivePolygonIndexesCount > 0)
                             if (model[ActiveUnitIndex[i]].CalculatePolygonNormals(true) != 0) throw new Exception(Graph3DLibrary.ErrorLog.GetLastError());
                 });
                fpsStatistics.NextPoint("Расчет нормалей к полигонам");
                #endregion

                #region Фильтруем невидимые полигоны (часть 2)                
                Parallel.For(0, ActiveUnitIndexCount, (int i) =>
                 {
                     if (model[ActiveUnitIndex[i]] != null)
                         if (model[ActiveUnitIndex[i]].ActivePolygonIndexesCount > 0)
                         {
                             #region Фильтрация полигонов, отвернутых от сцены
                             if (model[ActiveUnitIndex[i]].FilterPolygonDirectedAwayFromScene() != 0) throw new Exception(Graph3DLibrary.ErrorLog.GetLastError());
                             #endregion
                         }
                 });
                fpsStatistics.NextPoint("Фильтруем невидимые полигоны (часть 2)");
                #endregion

                #region Сортировка моделей
                // внутри ModelController
                #endregion

                #region Сортировка полигонов моделей // параллельная обработка сокращает процессорное время с 21-22% до 7-9% (в 3 раза)                
                Parallel.For(0, ActiveUnitIndexCount, (int i) =>
                 {
                     if (model[ActiveUnitIndex[i]] != null)
                         if (model[ActiveUnitIndex[i]].ActivePolygonIndexesCount > 0)
                             if (model[ActiveUnitIndex[i]].SortActivePolygonIndexes() != 0) throw new Exception(Graph3DLibrary.ErrorLog.GetLastError());
                 });
                fpsStatistics.NextPoint("Сортировка полигонов моделей");
                #endregion

                #region Расчет освещения полигонов
                Parallel.For(0, ActiveUnitIndexCount, (int i) =>
                 {
                     if (model[ActiveUnitIndex[i]] != null)
                         if (model[ActiveUnitIndex[i]].ActivePolygonIndexesCount > 0)
                         {
                             #region Расчет нормалей для расчета освещения
                             if (model[ActiveUnitIndex[i]].CalculatePolygonNormals(false) != 0) throw new Exception(Graph3DLibrary.ErrorLog.GetLastError());
                             #endregion

                             #region Сброс освещения
                             if (model[ActiveUnitIndex[i]].ResetLighting() != 0) throw new Exception(Graph3DLibrary.ErrorLog.GetLastError());
                             #endregion

                             for (int j = 0; j < DirectLightColor.Length; j++)
                                 if (model[ActiveUnitIndex[i]].AddLight(DirectVectorLight[j], DirectLightColor[j], DirectLightPower[j], LightTypes.Directional) != 0) throw new Exception(Graph3DLibrary.ErrorLog.GetLastError());
                             if (model[ActiveUnitIndex[i]].AddLight(null, Color.FromArgb(255, AmbientColorR, AmbientColorG, AmbientColorB), AmbientLightPower, LightTypes.Ambient) != 0) throw new Exception(Graph3DLibrary.ErrorLog.GetLastError());
                         }
                 });
                fpsStatistics.NextPoint("Расчет освещения полигонов");
                #endregion

                #endregion

                #region Слияние стеков полигонов в контроллер коллекций                
                if (modelController.MergeActivePolygon() != 0) throw new Exception(ErrorLog.GetLastError());
                fpsStatistics.NextPoint("Слияние стеков полигонов в контроллер коллекций");
                #endregion

                #region Выбор объекта                
                {
                    if (MouseSelectedObjectIdx == -1)
                    {
                        PointF[] p = new PointF[3];
                        PointF pTest = new PointF();

                        for (int i = modelController.CollectionActivePolygonCount - 1; i >= 0; i--)
                        {

                            if (modelController.ActivePolygonBuffer[i].ModelIndex < CoordLineCount + SelectionLineCount) continue;

                            pTest.X = MouseSelectPos.X;
                            pTest.Y = MouseSelectPos.Y;

                            for (int j = 0; j < 3; j++)
                            {
                                p[j].X = model[modelController.ActivePolygonBuffer[i].ModelIndex].
                                        ScreenVertex3D[
                                            model[modelController.ActivePolygonBuffer[i].ModelIndex].Polygon[modelController.ActivePolygonBuffer[i].PolygonIndex].PointIndex[j]
                                                      ].X;
                                p[j].Y = model[modelController.ActivePolygonBuffer[i].ModelIndex].
                                        ScreenVertex3D[
                                            model[modelController.ActivePolygonBuffer[i].ModelIndex].Polygon[modelController.ActivePolygonBuffer[i].PolygonIndex].PointIndex[j]
                                                      ].Y;
                            }

                            if (Engine3D.TestPointInTrangle(p[0], p[1], p[2], pTest))
                            {
                                MouseSelectedObjectIdx = modelController.ActivePolygonBuffer[i].ModelIndex;
                                MouseSelectObject();
                                break;
                            }

                        }
                        MouseSelectedObjectIdx = -2;
                    }
                }
                fpsStatistics.NextPoint("Выбор объекта");
                #endregion

                #region Отрисовка сцены
                switch (ModelDrawType)
                {
                    case 0:
                        if (modelController.ShowWideModel(Surface, modelController.ClosedSurfaceModel ? PolygonSides.Auto : PolygonSides.AllSides) != 0) throw new Exception(ErrorLog.GetLastError());
                        break;
                    case 1:
                        if (modelController.ShowPolygonModel(Surface, modelController.ClosedSurfaceModel ? PolygonSides.Auto : PolygonSides.AllSides) != 0) throw new Exception(ErrorLog.GetLastError());
                        break;
                    default:
                        if (modelController.ShowModel(Surface, modelController.ClosedSurfaceModel ? PolygonSides.Auto : PolygonSides.AllSides) != 0) throw new Exception(ErrorLog.GetLastError());
                        break;
                }
                Surface.MergeBuffers();
                fpsStatistics.NextPoint("Отрисовка сцены");
                #endregion



                #region Сохранение в растровый файл                
                if (ScreenshotMode)
                    try
                    {
                        string fNamePart = ScreenshotFileName + string.Format("{0:d" + (ScreenshotSeries ? ScreenshotSeriesFileMask.ToString() : "3") + "}", ScreenshotSeriesFileNum++);
                        if (fNamePart.Trim() == "") fNamePart = "Screenshot";
                        string fName = ScreenshotFolder + "\\" + fNamePart + ".png";

                        Surface.SaveScreenshoot(fName, 0, System.Drawing.Imaging.ImageFormat.Png, ScreenshotFrame ? ScreenshotSize.X : 0, ScreenshotFrame ? ScreenshotSize.Y : 0);
     
                        if (!ScreenshotSeries)
                        {
                            ScreenshotMode = false;
                            MessageBox.Show("Снимок успешно выполнен");
                        }
                        else
                        {
                            if (ScreenshotSeriesPos >= Engine3D.Radian360)
                            {
                                ScreenshotSeriesPos = 0;
                                ScreenshotSeries = false;
                                ScreenshotMode = false;
                                MessageBox.Show("Снимки успешно выполнены");
                            }
                        }
                    }
                    catch (Exception er)
                    {
                        ScreenshotMode = false;
                        MessageBox.Show(er.Message);
                    }
                #endregion

                #region Подписи осей координат
                if (координатныеОсиToolStripMenuItem.Checked)
                {
                    SolidBrush brush = new SolidBrush(Color.Black);
                    Surface.DrawString("X", frmMain.DefaultFont, brush, CoordLineTopCamera[0].X, CoordLineTopCamera[0].Y,0);
                    Surface.DrawString("Y", frmMain.DefaultFont, brush, CoordLineTopCamera[1].X, CoordLineTopCamera[1].Y,0);
                    Surface.DrawString("Z", frmMain.DefaultFont, brush, CoordLineTopCamera[2].X, CoordLineTopCamera[2].Y,0);
                }
                #endregion

                #region Рамка
                if (WindowInFrame & (!ScreenshotMode))
                {
                    Pen pn = new Pen(Color.Black);
                    Surface.DrawRectangle(pn, new Rectangle((Surface.Width - WindowFrame.X) / 2, (Surface.Height - WindowFrame.Y) / 2, WindowFrame.X, WindowFrame.Y),0);
                }
                #endregion

                #endregion cоздание и отрисовка сцены  

                fpsStatistics.NextPoint("Вспомогательные действия");

                #region Подписи
                if (TechInfoToolStripMenuItem.Checked)
                {
                    string FPSText = "";
                    for (int i = 1; i < fpsStatistics.CurrentTimePoint; i++)
                        FPSText += $"\n\t{fpsStatistics.PointDescription[i]}: { fpsStatistics.Percent(i)}%";

                    SolidBrush brush = new SolidBrush(Color.Black);
                    Surface.DrawString("Координаты камеры: x:" + CameraPos.X.ToString() +
                                                              "; y:" + CameraPos.Y.ToString() +
                                                              "; z:" + CameraPos.Z.ToString() +
                                                              "; \nКоличество полигонов активных/всего: " + (modelController.CollectionActivePolygonCount - 2088).ToString() +
                                                              " / " + (modelController.ActivePolygonBuffer.Length - 2088).ToString() +
                                                              "\nFPS: " + fpsStatistics.FPS().ToString() +
                                                              FPSText+
                                                              $"\n\tКоличество буферов: {SurfaceBufferCount}"
                        , frmMain.DefaultFont, brush, 0, 0,0);
                }
                #endregion

                #region Переключаем кадр
                Surface.Render();
                #endregion

                timerDraw.Enabled = true;
            }
            catch (Exception er)
            {
                ErrorProtocol("timerDraw_Tick", er.Message);
            }
        }

        public void ModifyModel(VolumetricModel3D _model, MeshModifications info)
        {
            try
            {
                for (int j = 0; j < info.CurrentLength; j++)
                {
                    switch (info.MeshInfo[j].ModifyCode)
                    {
                        case 0:
                            if (_model.Move(info.MeshInfo[j].floatParameters[0],
                                            info.MeshInfo[j].floatParameters[1],
                                            info.MeshInfo[j].floatParameters[2]) != 0) throw new Exception(ErrorLog.GetLastError());
                            break;
                        case 1:
                            if (_model.Zoom(info.MeshInfo[j].floatParameters[0] / 100,
                                            info.MeshInfo[j].floatParameters[1] / 100,
                                            info.MeshInfo[j].floatParameters[2] / 100) != 0) throw new Exception(ErrorLog.GetLastError());
                            break;
                        case 2:
                            if (_model.Rotate(info.MeshInfo[j].floatParameters[0],
                                              (Axis3D)info.MeshInfo[j].intParameters[0]) != 0) throw new Exception(ErrorLog.GetLastError());
                            break;
                        case 3:
                            {
                                Point3D minP = new Point3D();
                                Point3D maxP = new Point3D();
                                if (_model.MinMaxVertexValue(ref minP, ref maxP, 1) != 0) throw new Exception(ErrorLog.GetLastError());

                                switch (info.MeshInfo[j].intParameters[0])
                                {
                                    case 0:
                                        if (_model.Move(-(minP.X + maxP.X) / 2, -(minP.Y + maxP.Y) / 2, -(minP.Z + maxP.Z) / 2) != 0) throw new Exception(ErrorLog.GetLastError());
                                        break;
                                    case 1:
                                        if (_model.Move(0, -minP.Y, 0) != 0) throw new Exception(ErrorLog.GetLastError());
                                        break;
                                    case 2:
                                        if (_model.Move(-minP.X, 0, 0) != 0) throw new Exception(ErrorLog.GetLastError());
                                        break;
                                    case 3:
                                        if (_model.Move(0, 0, -minP.Z) != 0) throw new Exception(ErrorLog.GetLastError());
                                        break;
                                }
                            }
                            break;
                        case 4:
                            {
                                float deltaLevel = info.MeshInfo[j].floatParameters[3] - info.MeshInfo[j].floatParameters[2];
                                float angleBottom = info.MeshInfo[j].floatParameters[0];
                                float angleTop = info.MeshInfo[j].floatParameters[1];
                                float deltaAngle = angleTop - angleBottom;
                                float angle = 0;

                                if (deltaLevel > 0)
                                    for (int i = 0; i < _model.CameraVertex3D.Length; i++)
                                    {
                                        if (_model.CameraVertex3D[i].Y > info.MeshInfo[j].floatParameters[3])
                                        {
                                            if (info.MeshInfo[j].intParameters[1] == 0) continue; else angle = angleTop;
                                        }
                                        else
                                            if (_model.CameraVertex3D[i].Y < info.MeshInfo[j].floatParameters[2])
                                        {
                                            if (info.MeshInfo[j].intParameters[0] == 0) continue; else angle = angleBottom;
                                        }
                                        else
                                            angle = (_model.CameraVertex3D[i].Y - info.MeshInfo[j].floatParameters[2]) / deltaLevel * (deltaAngle) + angleBottom;
                                        if (Engine3D.RotatePoint3D(angle, Axis3D.OYxz, _model.CameraVertex3D[i]) != 0) throw new Exception(ErrorLog.GetLastError());
                                    }
                            }
                            break;
                        case 5:
                            {
                                float deltaLevel = info.MeshInfo[j].floatParameters[2] - info.MeshInfo[j].floatParameters[1];
                                float angleBottom = 0;
                                float angleTop = info.MeshInfo[j].floatParameters[0];
                                float deltaAngle = angleTop - angleBottom;
                                float angle = 0;

                                if (deltaLevel > 0)
                                    for (int i = 0; i < _model.CameraVertex3D.Length; i++)
                                    {
                                        if (_model.CameraVertex3D[i].Y > info.MeshInfo[j].floatParameters[2])
                                        {
                                            angle = angleTop;
                                        }
                                        else
                                            if (_model.CameraVertex3D[i].Y < info.MeshInfo[j].floatParameters[1])
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            angle = (_model.CameraVertex3D[i].Y - info.MeshInfo[j].floatParameters[1]) / deltaLevel * (deltaAngle) + angleBottom;
                                        }

                                        if (Engine3D.RotatePoint3D(angle, Axis3D.OZyx, _model.CameraVertex3D[i]) != 0) throw new Exception(ErrorLog.GetLastError());

                                    }
                            }
                            break;
                        case 6:
                            {
                                float Rx = info.MeshInfo[j].floatParameters[1] - info.MeshInfo[j].floatParameters[0];
                                float r = 0;
                                float dY1 = info.MeshInfo[j].floatParameters[6] / 100;
                                float dY2 = info.MeshInfo[j].floatParameters[7] / 100;

                                if (Rx < 1) break;

                                for (int i = 0; i < _model.CameraVertex3D.Length; i++)
                                {
                                    if (_model.CameraVertex3D[i].X < info.MeshInfo[j].floatParameters[0]) continue;
                                    if (_model.CameraVertex3D[i].X > info.MeshInfo[j].floatParameters[1]) continue;

                                    if (_model.CameraVertex3D[i].Y < info.MeshInfo[j].floatParameters[2]) continue;
                                    if (_model.CameraVertex3D[i].Y > info.MeshInfo[j].floatParameters[3]) continue;

                                    if (_model.CameraVertex3D[i].Z < info.MeshInfo[j].floatParameters[4]) continue;
                                    if (_model.CameraVertex3D[i].Z > info.MeshInfo[j].floatParameters[5]) continue;

                                    r = (info.MeshInfo[j].floatParameters[1] - _model.CameraVertex3D[i].X) / Rx;

                                    _model.CameraVertex3D[i].Y = _model.CameraVertex3D[i].Y * (r * dY2 + (1 - r) * dY1);
                                }
                            }
                            break;
                            /*                            
                        case 7:
                            #region Сферический градиент
                            {
                                float dist = 0;
                                float sphInR = info.MeshInfo[j].floatParameters[3];
                                float sphR = info.MeshInfo[j].floatParameters[4];
                                float delta = 0;
                                int side = 0;
                                PolygonSides ColorSide = PolygonSides.FrontSide;
                                switch (info.MeshInfo[j].intParameters[8])
                                {
                                    case 0:
                                        side = 0;
                                        ColorSide = PolygonSides.FrontSide;
                                        break;
                                    case 1:
                                        side = 1;
                                        ColorSide = PolygonSides.RearSide;
                                        break;
                                    default:
                                        side = 0;
                                        ColorSide = PolygonSides.FrontSide;
                                        break;

                                }

                                float TotalOpacity = info.MeshInfo[j].floatParameters[7]/100;
                                Point3D sphC = new Point3D(info.MeshInfo[j].floatParameters[0], info.MeshInfo[j].floatParameters[1], info.MeshInfo[j].floatParameters[2]);
                                if ((sphR < 1) | (sphInR < 0) | (sphInR >= sphR)) break;
                                for (int i = 0; i < _model.Polygon.Length; i++)
                                {
                                    dist = Engine3D.GetDistance(sphC, _model.Polygon[i].Center);

                                    if (dist > sphR)
                                    {
                                        if (info.MeshInfo[j].intParameters[1]==0) continue;
                                        delta = 1;
                                    }
                                    else if (dist < sphInR)
                                    {
                                        if (info.MeshInfo[j].intParameters[0] == 0) continue;
                                        delta = 0;
                                    }
                                    else delta = (dist - sphInR) / (sphR - sphInR);

                                    if (_model.SetColor(Color.FromArgb((int)Math.Round(_model.Polygon[i].color[side].A * (1 - TotalOpacity) + TotalOpacity * 2.55 * (info.MeshInfo[j].floatParameters[5] + (info.MeshInfo[j].floatParameters[6] - info.MeshInfo[j].floatParameters[5]) * delta)),
                                                                       (int)Math.Round(_model.Polygon[i].color[side].R * (1 - TotalOpacity) + TotalOpacity * (info.MeshInfo[j].intParameters[2] + (info.MeshInfo[j].intParameters[5] - info.MeshInfo[j].intParameters[2]) * delta)),
                                                                       (int)Math.Round(_model.Polygon[i].color[side].G * (1 - TotalOpacity) + TotalOpacity * (info.MeshInfo[j].intParameters[3] + (info.MeshInfo[j].intParameters[6] - info.MeshInfo[j].intParameters[3]) * delta)),
                                                                       (int)Math.Round(_model.Polygon[i].color[side].B * (1 - TotalOpacity) + TotalOpacity * (info.MeshInfo[j].intParameters[4] + (info.MeshInfo[j].intParameters[7] - info.MeshInfo[j].intParameters[4]) * delta))), i, i, ColorSide) != 0) throw new Exception(ErrorLog.GetLastError());
                                    if (info.MeshInfo[j].intParameters[8]==2)
                                    {
                                        if (_model.SetColor(Color.FromArgb((int)Math.Round(_model.Polygon[i].color[1].A * (1 - TotalOpacity) + TotalOpacity * 2.55 * (info.MeshInfo[j].floatParameters[5] + (info.MeshInfo[j].floatParameters[6] - info.MeshInfo[j].floatParameters[5]) * delta)),
                                                                       (int)Math.Round(_model.Polygon[i].color[1].R * (1 - TotalOpacity) + TotalOpacity * (info.MeshInfo[j].intParameters[2] + (info.MeshInfo[j].intParameters[5] - info.MeshInfo[j].intParameters[2]) * delta)),
                                                                       (int)Math.Round(_model.Polygon[i].color[1].G * (1 - TotalOpacity) + TotalOpacity * (info.MeshInfo[j].intParameters[3] + (info.MeshInfo[j].intParameters[6] - info.MeshInfo[j].intParameters[3]) * delta)),
                                                                       (int)Math.Round(_model.Polygon[i].color[1].B * (1 - TotalOpacity) + TotalOpacity * (info.MeshInfo[j].intParameters[4] + (info.MeshInfo[j].intParameters[7] - info.MeshInfo[j].intParameters[4]) * delta))), i, i, PolygonSides.RearSide) != 0) throw new Exception(ErrorLog.GetLastError());
                                    }
                                }
                            }
                            #endregion
                            break;
                            */
                    }
                }
            }
            catch (Exception er)
            {
                ErrorProtocol("ModifyModel", er.Message);
            }
        }

        public void MouseSelectObject(bool MoveSelectionOnly = false)
        {
            try
            {
                if (MouseSelectedObjectIdx < 0) return;

                Point3D minVal = new Point3D();
                Point3D maxVal = new Point3D();
                Point3D centerVal = new Point3D();

                #region Перемещение стрелок
                model[MouseSelectedObjectIdx].MinMaxVertexValue(ref minVal, ref maxVal, 1);
                model[MouseSelectedObjectIdx].CenterVertexValue(ref SelectionObjectPosition, 1);
                float delta = SelectionSectionCount / 2 * SelectionSectionHeight;
                for (int j = 0; j < ModelSelection.Length; j++)
                {
                    ModelSelection[j].SetColor(Color.FromArgb(100, Color.Red.R, Color.Red.G, Color.Red.B));
                    ModelSelection[j].ResetCameraModel();
                    ModelSelection[j].CenterVertexValue(ref centerVal, 1);
                    centerVal = centerVal.GetRound();

                    switch (j)
                    {
                        case 0:
                            ModelSelection[j].Move(minVal.X - centerVal.X - delta, (maxVal.Y + minVal.Y) / 2 - centerVal.Y, (maxVal.Z + minVal.Z) / 2 - centerVal.Z);
                            break;
                        case 1:
                            ModelSelection[j].Move((maxVal.X + minVal.X) / 2 - centerVal.X, minVal.Y - centerVal.Y - delta, (maxVal.Z + minVal.Z) / 2 - centerVal.Z);
                            break;
                        case 2:
                            ModelSelection[j].Move((maxVal.X + minVal.X) / 2 - centerVal.X, (maxVal.Y + minVal.Y) / 2 - centerVal.Y, minVal.Z - centerVal.Z - delta);
                            break;
                        case 3:
                            ModelSelection[j].Move(maxVal.X - centerVal.X + delta, (maxVal.Y + minVal.Y) / 2 - centerVal.Y, (maxVal.Z + minVal.Z) / 2 - centerVal.Z);
                            break;
                        case 4:
                            ModelSelection[j].Move((maxVal.X + minVal.X) / 2 - centerVal.X, maxVal.Y - centerVal.Y + delta, (maxVal.Z + minVal.Z) / 2 - centerVal.Z);
                            break;
                        case 5:
                            ModelSelection[j].Move((maxVal.X + minVal.X) / 2 - centerVal.X, (maxVal.Y + minVal.Y) / 2 - centerVal.Y, maxVal.Z - centerVal.Z + delta);
                            break;

                    }

                    ModelSelection[j].SaveCameraModelToMain();
                }

                #region Позиция выбранного объекта
                statusStrip1.Items[1].Text = (MouseSelectedObjectIdx >= 0) ? ("Позиция: (" + SelectionObjectPosition.X.ToString("0.00", CultureInfo.InvariantCulture) + "; " + SelectionObjectPosition.Y.ToString("0.00", CultureInfo.InvariantCulture) + "; " + SelectionObjectPosition.Z.ToString("0.00", CultureInfo.InvariantCulture) + ")") : "";
                #endregion

                #region Размеры выбранного объекта
                statusStrip1.Items[2].Text = (MouseSelectedObjectIdx >= 0) ? ("Размер: (" + (maxVal.X - minVal.X).ToString("0.00", CultureInfo.InvariantCulture) + "; " + (maxVal.Y - minVal.Y).ToString("0.00", CultureInfo.InvariantCulture) + "; " + (maxVal.Z - minVal.Z).ToString("0.00", CultureInfo.InvariantCulture) + ")") : "";
                #endregion

                SelectedObjectIdx = MouseSelectedObjectIdx;
                TabsheetProperties.Enabled = true;
                MouseSelectedObjectIdx = -2;
                #endregion

                if (MoveSelectionOnly) return;

                VisualComponentMethodLock++;

                #region Описание
                if (SelectedObjectIdx >= 0)
                {
                    #region Наименование


                    #region Выбранный объект
                    VisualComponentMethodLock++;
                    cmbMesh.SelectedIndex = SelectedObjectIdx - CoordLineCount - SelectionLineCount;
                    VisualComponentMethodLock--;
                    statusStrip1.Items[0].Text = (cmbMesh.SelectedIndex >= 0) ? ("Выбран объект № " + (SelectedObjectIdx - CoordLineCount - SelectionLineCount).ToString() + "; ") : "";
                    #endregion

                    #endregion

                    #region Опорные точки
                    nudSettingN1.Enabled = false;
                    nudSettingN2.Enabled = false;
                    nudSettingN3.Enabled = false;

                    switch (model[SelectedObjectIdx].ModelType())
                    {
                        case ModelTypes.Plane3D:
                            nudSettingN1.Enabled = true;
                            nudSettingN1.Value = ((Plane3D)model[SelectedObjectIdx]).N;
                            break;
                        case ModelTypes.CellPlane3D:
                            nudSettingN1.Enabled = true;
                            nudSettingN2.Enabled = true;
                            nudSettingN1.Value = ((CellPlane3D)model[SelectedObjectIdx])._Nx;
                            nudSettingN2.Value = ((CellPlane3D)model[SelectedObjectIdx])._Nz;
                            break;
                        case ModelTypes.Cylinder3D:
                            nudSettingN1.Enabled = true;
                            nudSettingN2.Enabled = true;
                            nudSettingN1.Value = ((Cylinder3D)model[SelectedObjectIdx]).N;
                            nudSettingN2.Value = ((Cylinder3D)model[SelectedObjectIdx]).sectionN;
                            break;
                        case ModelTypes.Ellipse3D:
                            nudSettingN1.Enabled = true;
                            nudSettingN2.Enabled = true;
                            nudSettingN1.Value = ((Ellipse3D)model[SelectedObjectIdx]).Nx;
                            nudSettingN2.Value = ((Ellipse3D)model[SelectedObjectIdx]).Nz;
                            break;
                        case ModelTypes.Tor3D:
                            nudSettingN1.Enabled = true;
                            nudSettingN2.Enabled = true;
                            nudSettingN1.Value = ((Tor3D)model[SelectedObjectIdx]).Nx;
                            nudSettingN2.Value = ((Tor3D)model[SelectedObjectIdx]).Nz;
                            break;
                        case ModelTypes.Prism3D:
                            nudSettingN1.Enabled = true;
                            nudSettingN2.Enabled = true;
                            nudSettingN3.Enabled = true;
                            nudSettingN1.Value = ((Prism3D)model[SelectedObjectIdx]).nWidth;
                            nudSettingN2.Value = ((Prism3D)model[SelectedObjectIdx]).nHeight;
                            nudSettingN3.Value = ((Prism3D)model[SelectedObjectIdx]).nDepth;
                            break;
                        case ModelTypes.SurfaceHole3D:
                            nudSettingN1.Enabled = true;
                            nudSettingN2.Enabled = true;
                            nudSettingN1.Value = ((SurfaceHole3D)model[SelectedObjectIdx]).Nx;
                            nudSettingN2.Value = ((SurfaceHole3D)model[SelectedObjectIdx]).Nz;
                            break;
                    }
                    #endregion

                    #region Углы
                    nudAngle1Start.Enabled = false;
                    nudAngle1Finish.Enabled = false;
                    nudAngle2Start.Enabled = false;
                    nudAngle2Finish.Enabled = false;

                    switch (model[SelectedObjectIdx].ModelType())
                    {
                        case ModelTypes.Plane3D:
                            nudAngle1Start.Enabled = true;
                            nudAngle1Finish.Enabled = true;
                            nudAngle1Start.Value = (decimal)(((Plane3D)model[SelectedObjectIdx]).AngleStart / Engine3D.RadianDegrees);
                            nudAngle1Finish.Value = (decimal)(((Plane3D)model[SelectedObjectIdx]).AngleFinish / Engine3D.RadianDegrees);
                            break;
                        case ModelTypes.Cylinder3D:
                            nudAngle1Start.Enabled = true;
                            nudAngle1Finish.Enabled = true;
                            nudAngle1Start.Value = (decimal)(((Cylinder3D)model[SelectedObjectIdx]).AngleStart / Engine3D.RadianDegrees);
                            nudAngle1Finish.Value = (decimal)(((Cylinder3D)model[SelectedObjectIdx]).AngleFinish / Engine3D.RadianDegrees);
                            break;
                        case ModelTypes.Ellipse3D:
                            nudAngle1Start.Enabled = true;
                            nudAngle1Finish.Enabled = true;
                            nudAngle2Start.Enabled = true;
                            nudAngle2Finish.Enabled = true;
                            nudAngle1Start.Value = (decimal)(((Ellipse3D)model[SelectedObjectIdx]).StartAngleXY / Engine3D.RadianDegrees);
                            nudAngle1Finish.Value = (decimal)(((Ellipse3D)model[SelectedObjectIdx]).FinishAngleXY / Engine3D.RadianDegrees);
                            nudAngle2Start.Value = (decimal)(((Ellipse3D)model[SelectedObjectIdx]).StartAngleYZ / Engine3D.RadianDegrees);
                            nudAngle2Finish.Value = (decimal)(((Ellipse3D)model[SelectedObjectIdx]).FinishAngleYZ / Engine3D.RadianDegrees);
                            break;
                        case ModelTypes.Tor3D:
                            nudAngle1Start.Enabled = true;
                            nudAngle1Finish.Enabled = true;
                            nudAngle2Start.Enabled = true;
                            nudAngle2Finish.Enabled = true;
                            nudAngle1Start.Value = (decimal)(((Tor3D)model[SelectedObjectIdx]).StartAngleXY / Engine3D.RadianDegrees);
                            nudAngle1Finish.Value = (decimal)(((Tor3D)model[SelectedObjectIdx]).FinishAngleXY / Engine3D.RadianDegrees);
                            nudAngle2Start.Value = (decimal)(((Tor3D)model[SelectedObjectIdx]).StartAngleYZ / Engine3D.RadianDegrees);
                            nudAngle2Finish.Value = (decimal)(((Tor3D)model[SelectedObjectIdx]).FinishAngleYZ / Engine3D.RadianDegrees);
                            break;
                    }
                    #endregion

                    #region Поверхности
                    chkBottom.Enabled = false;
                    chkTop.Enabled = false;
                    chkLeft.Enabled = false;
                    chkRight.Enabled = false;
                    chkFront.Enabled = false;
                    chkRear.Enabled = false;

                    switch (model[SelectedObjectIdx].ModelType())
                    {
                        case ModelTypes.Cylinder3D:
                            chkBottom.Enabled = true;
                            chkTop.Enabled = true;
                            chkBottom.Checked = ((Cylinder3D)model[SelectedObjectIdx]).BottomVisible;
                            chkTop.Checked = ((Cylinder3D)model[SelectedObjectIdx]).TopVisible;
                            break;
                        case ModelTypes.Prism3D:
                            chkBottom.Enabled = true;
                            chkTop.Enabled = true;
                            chkLeft.Enabled = true;
                            chkRight.Enabled = true;
                            chkFront.Enabled = true;
                            chkRear.Enabled = true;
                            chkBottom.Checked = ((Prism3D)model[SelectedObjectIdx]).BottomVisible;
                            chkTop.Checked = ((Prism3D)model[SelectedObjectIdx]).TopVisible;
                            chkLeft.Checked = ((Prism3D)model[SelectedObjectIdx]).LeftVisible;
                            chkRight.Checked = ((Prism3D)model[SelectedObjectIdx]).RightVisible;
                            chkFront.Checked = ((Prism3D)model[SelectedObjectIdx]).FrontVisible;
                            chkRear.Checked = ((Prism3D)model[SelectedObjectIdx]).RearVisible;
                            break;
                    }
                    #endregion

                    #region Начальные размеры


                    NudR1Xplus.Enabled = false;
                    NudR1Xminus.Enabled = false;
                    NudR1Yplus.Enabled = false;
                    NudR1Yminus.Enabled = false;
                    NudR1Zplus.Enabled = false;
                    NudR1Zminus.Enabled = false;
                    NudR2Xplus.Enabled = false;
                    NudR2Xminus.Enabled = false;
                    NudR2Yplus.Enabled = false;
                    NudR2Yminus.Enabled = false;
                    NudR2Zplus.Enabled = false;
                    NudR2Zminus.Enabled = false;

                    switch (model[SelectedObjectIdx].ModelType())
                    {
                        case ModelTypes.Plane3D:
                            NudR1Xplus.Enabled = true;
                            NudR1Xminus.Enabled = true;
                            NudR1Yplus.Enabled = true;
                            NudR1Yminus.Enabled = true;
                            NudR1Xplus.Value = (decimal)((Plane3D)model[SelectedObjectIdx]).RxPlus;
                            NudR1Xminus.Value = (decimal)((Plane3D)model[SelectedObjectIdx]).RxMinus;
                            NudR1Yplus.Value = (decimal)((Plane3D)model[SelectedObjectIdx]).RyPlus;
                            NudR1Yminus.Value = (decimal)((Plane3D)model[SelectedObjectIdx]).RyMinus;
                            break;
                        case ModelTypes.CellPlane3D:
                            NudR1Xplus.Enabled = true;
                            NudR1Zplus.Enabled = true;
                            NudR1Xplus.Value = (decimal)((CellPlane3D)model[SelectedObjectIdx])._WidthX;
                            NudR1Zplus.Value = (decimal)((CellPlane3D)model[SelectedObjectIdx])._WidthZ;
                            break;
                        case ModelTypes.Cylinder3D:
                            NudR1Xplus.Enabled = true;
                            NudR1Xminus.Enabled = true;
                            NudR1Zplus.Enabled = true;
                            NudR1Zminus.Enabled = true;
                            NudR2Xplus.Enabled = true;
                            NudR2Xminus.Enabled = true;
                            NudR2Zplus.Enabled = true;
                            NudR2Zminus.Enabled = true;
                            NudR1Yplus.Enabled = true;
                            NudR1Xplus.Value = (decimal)((Cylinder3D)model[SelectedObjectIdx]).RBottomXpositive;
                            NudR1Xminus.Value = (decimal)((Cylinder3D)model[SelectedObjectIdx]).RBottomXnegative;
                            NudR1Zplus.Value = (decimal)((Cylinder3D)model[SelectedObjectIdx]).RBottomZpositive;
                            NudR1Zminus.Value = (decimal)((Cylinder3D)model[SelectedObjectIdx]).RBottomZnegative;
                            NudR2Xplus.Value = (decimal)((Cylinder3D)model[SelectedObjectIdx]).RTopXpositive;
                            NudR2Xminus.Value = (decimal)((Cylinder3D)model[SelectedObjectIdx]).RTopXnegative;
                            NudR2Zplus.Value = (decimal)((Cylinder3D)model[SelectedObjectIdx]).RTopZpositive;
                            NudR2Zminus.Value = (decimal)((Cylinder3D)model[SelectedObjectIdx]).RTopZnegative;
                            NudR1Yplus.Value = (decimal)((Cylinder3D)model[SelectedObjectIdx]).Height;
                            break;
                        case ModelTypes.Prism3D:
                            NudR1Xplus.Enabled = true;
                            NudR1Yplus.Enabled = true;
                            NudR1Zplus.Enabled = true;
                            NudR1Xplus.Value = (decimal)((Prism3D)model[SelectedObjectIdx]).Width;
                            NudR1Yplus.Value = (decimal)((Prism3D)model[SelectedObjectIdx]).Height;
                            NudR1Zplus.Value = (decimal)((Prism3D)model[SelectedObjectIdx]).Depth;
                            break;
                        case ModelTypes.Ellipse3D:
                            NudR1Xplus.Enabled = true;
                            NudR1Xminus.Enabled = true;
                            NudR1Yplus.Enabled = true;
                            NudR1Yminus.Enabled = true;
                            NudR1Zplus.Enabled = true;
                            NudR1Zminus.Enabled = true;
                            NudR1Xplus.Value = (decimal)((Ellipse3D)model[SelectedObjectIdx]).RxPositive;
                            NudR1Xminus.Value = (decimal)((Ellipse3D)model[SelectedObjectIdx]).RxNegative;
                            NudR1Yplus.Value = (decimal)((Ellipse3D)model[SelectedObjectIdx]).RyPositive;
                            NudR1Yminus.Value = (decimal)((Ellipse3D)model[SelectedObjectIdx]).RyNegative;
                            NudR1Zplus.Value = (decimal)((Ellipse3D)model[SelectedObjectIdx]).RzPositive;
                            NudR1Zminus.Value = (decimal)((Ellipse3D)model[SelectedObjectIdx]).RzNegative;
                            break;
                        case ModelTypes.Tor3D:
                            NudR1Xplus.Enabled = true;
                            NudR1Xminus.Enabled = true;
                            NudR1Yplus.Enabled = true;
                            NudR1Yminus.Enabled = true;
                            NudR2Xplus.Enabled = true;
                            NudR2Xminus.Enabled = true;
                            NudR2Yplus.Enabled = true;
                            NudR2Yminus.Enabled = true;
                            NudR2Zplus.Enabled = true;
                            NudR2Zminus.Enabled = true;
                            NudR1Xplus.Value = (decimal)((Tor3D)model[SelectedObjectIdx]).RxPositive;
                            NudR1Xminus.Value = (decimal)((Tor3D)model[SelectedObjectIdx]).RxNegative;
                            NudR1Yplus.Value = (decimal)((Tor3D)model[SelectedObjectIdx]).RyPositive;
                            NudR1Yminus.Value = (decimal)((Tor3D)model[SelectedObjectIdx]).RyNegative;
                            NudR2Xplus.Value = (decimal)((Tor3D)model[SelectedObjectIdx]).RADxPositive;
                            NudR2Xminus.Value = (decimal)((Tor3D)model[SelectedObjectIdx]).RADxNegative;
                            NudR2Yplus.Value = (decimal)((Tor3D)model[SelectedObjectIdx]).RADyPositive;
                            NudR2Yminus.Value = (decimal)((Tor3D)model[SelectedObjectIdx]).RADyNegative;
                            NudR2Zplus.Value = (decimal)((Tor3D)model[SelectedObjectIdx]).RADzPositive;
                            NudR2Zminus.Value = (decimal)((Tor3D)model[SelectedObjectIdx]).RADzNegative;
                            break;
                        case ModelTypes.SurfaceHole3D:
                            NudR1Xplus.Enabled = true;
                            NudR1Xminus.Enabled = true;
                            NudR1Zplus.Enabled = true;
                            NudR1Zminus.Enabled = true;
                            NudR1Xplus.Value = (decimal)((SurfaceHole3D)model[SelectedObjectIdx]).RXpositive;
                            NudR1Xminus.Value = (decimal)((SurfaceHole3D)model[SelectedObjectIdx]).RXnegative;
                            NudR1Zplus.Value = (decimal)((SurfaceHole3D)model[SelectedObjectIdx]).RZpositive;
                            NudR1Zminus.Value = (decimal)((SurfaceHole3D)model[SelectedObjectIdx]).RZnegative;
                            break;
                    }

                    #endregion

                    #region Материалы
                    lblColor1.Visible = false;
                    lblColor2.Visible = false;
                    panelColor1.Enabled = true;
                    panelColor2.Enabled = true;
                    nudColorExtCode.Enabled = true;
                    nudColorIntCode.Enabled = true;

                    trackMatte1.Visible = true;
                    trackMatte2.Visible = true;
                    lblMatte1.Visible = false;
                    lblMatte2.Visible = false;

                    cmbFillType.Visible = true;
                    lblFillType.Visible = false;

                    chkDoubleSided.Enabled = true;

                    lblOpacityMul.Visible = false;
                    trackOpacity.Visible = true;
                    trackOpacity.Enabled = true;

                    panelColor1.BackColor = model[SelectedObjectIdx].Polygon[0].color[0];
                    for (int i = 1; i < model[SelectedObjectIdx].Polygon.Length; i++)
                        if (model[SelectedObjectIdx].Polygon[i].color[0] != model[SelectedObjectIdx].Polygon[0].color[0])
                        {
                            lblColor1.Visible = true;
                            panelColor1.Enabled = false;
                            nudColorExtCode.Enabled = false;
                            panelColor1.BackColor = Color.LightGray;
                            break;
                        }

                    panelColor2.BackColor = model[SelectedObjectIdx].Polygon[0].color[1];
                    for (int i = 1; i < model[SelectedObjectIdx].Polygon.Length; i++)
                        if (model[SelectedObjectIdx].Polygon[i].color[1] != model[SelectedObjectIdx].Polygon[0].color[1])
                        {
                            lblColor2.Visible = true;
                            panelColor2.Enabled = false;
                            nudColorIntCode.Enabled = false;
                            panelColor2.BackColor = Color.LightGray;
                            break;
                        }
                    nudColorExtCode.Value = panelColor1.BackColor.ToArgb();
                    nudColorIntCode.Value = panelColor2.BackColor.ToArgb();

                    trackMatte1.Value = (int)Math.Round(model[SelectedObjectIdx].Polygon[0].Matte[0] * 100);
                    for (int i = 1; i < model[SelectedObjectIdx].Polygon.Length; i++)
                        if (model[SelectedObjectIdx].Polygon[i].Matte[0] != model[SelectedObjectIdx].Polygon[0].Matte[0])
                        {
                            trackMatte1.Visible = false;
                            lblMatte1.Visible = true;
                            break;
                        }

                    trackMatte2.Value = (int)Math.Round(model[SelectedObjectIdx].Polygon[0].Matte[1] * 100);
                    for (int i = 1; i < model[SelectedObjectIdx].Polygon.Length; i++)
                        if (model[SelectedObjectIdx].Polygon[i].Matte[1] != model[SelectedObjectIdx].Polygon[0].Matte[1])
                        {
                            trackMatte2.Visible = false;
                            lblMatte2.Visible = true;
                            break;
                        }

                    cmbFillType.SelectedIndex = (model[SelectedObjectIdx].Polygon[0].FillType == PolygonFillType.Solid) ? 0 : 1;
                    for (int i = 1; i < model[SelectedObjectIdx].Polygon.Length; i++)
                        if (model[SelectedObjectIdx].Polygon[i].FillType != model[SelectedObjectIdx].Polygon[0].FillType)
                        {
                            cmbFillType.Visible = false;
                            lblFillType.Visible = true;
                            break;
                        }

                    chkDoubleSided.Checked = model[SelectedObjectIdx].Polygon[0].DoubleSided;
                    for (int i = 1; i < model[SelectedObjectIdx].Polygon.Length; i++)
                        if (model[SelectedObjectIdx].Polygon[i].DoubleSided != model[SelectedObjectIdx].Polygon[0].DoubleSided)
                        {
                            chkDoubleSided.Enabled = false;
                            chkDoubleSided.CheckState = CheckState.Indeterminate;
                            break;
                        }

                    trackOpacity.Value = (int)Math.Round(((float)model[SelectedObjectIdx].Polygon[0].color[0].A * 100) / 255);
                    for (int i = 0; i < model[SelectedObjectIdx].Polygon.Length; i++)
                        if ((model[SelectedObjectIdx].Polygon[i].color[0].A != model[SelectedObjectIdx].Polygon[0].color[0].A) |
                            (model[SelectedObjectIdx].Polygon[i].color[1].A != model[SelectedObjectIdx].Polygon[0].color[0].A))
                        {
                            trackOpacity.Enabled = false;
                            trackOpacity.Visible = false;
                            lblOpacityVal.Text = "Непрозрачность:";
                            lblOpacityMul.Visible = true;
                            break;
                        }

                    chkClosedSurface.Checked = model[SelectedObjectIdx].ClosedSurface;

                    #endregion

                    if (trackOpacity.Enabled) trackOpacity_ValueChanged(null, null);

                    VisualComponentMethodLock--;

                    #region Модификации
                    MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].RefreshList(listModifers);
                    tabModProperties.Visible = (listModifers.Items.Count > 0);
                    if (listModifers.Items.Count > 0) listModifers.SelectedIndex = 0; else listModifers.SelectedIndex = -1;
                    listModifers_SelectedIndexChanged(null, null);
                    #endregion


                }
                #endregion
            }
            catch (Exception er)
            {
                ErrorProtocol("MouseSelectObject", er.Message);
            }
        }

        public void DeselectObject()
        {
            try
            {
                #region Наименование
                statusStrip1.Items[0].Text = "";
                VisualComponentMethodLock++;
                cmbMesh.SelectedIndex = -1;
                VisualComponentMethodLock--;
                #endregion

                #region Позиция
                statusStrip1.Items[1].Text = "";
                #endregion

                #region Размеры
                statusStrip1.Items[2].Text = "";
                #endregion

                SelectedObjectIdx = -1;
                MouseSelectedObjectIdx = -2;
                TabsheetProperties.Enabled = false;

                for (int i = 0; i < ModelSelection.Length; i++)
                    ModelSelection[i].SetColor(Color.FromArgb(0, Color.Red.R, Color.Red.G, Color.Red.B));

            }
            catch (Exception er)
            {
                ErrorProtocol("DeselectObject", er.Message);
            }
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Программа 'Студия трехмерных моделей'\nРазработал Егоров Денис Алексеевич\nг.Нижнекамск, 2018 год.\nЗамечания и пожелания направляйте по адресу:\n\tEgorovDADev@yandex.ru\nО новых программных продуктах и обновлениях Вы можете узнать на сайте:\n\tvk.com/it.shoping", "О программе..", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void frmMain_ResizeEnd(object sender, EventArgs e)
        {
            try
            {
                Surface?.DevalidationSurfaces();
                timerDraw.Enabled = true;
            }
            catch (Exception er)
            {
                MessageBox.Show("FrmMain_ResizeEnd\n" + er.Message);
            }
        }

        private void frmMain_ResizeBegin(object sender, EventArgs e)
        {
            timerDraw.Enabled = false;
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (((UnitCount - CoordLineCount - SelectionLineCount) > 0) & isEditedProject)
                switch (MessageBox.Show("Сохранить текущий проект?", "Внимание!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning))
                {
                    case DialogResult.Yes: сохранитьПроектКакToolStripMenuItem_Click(null, null);
                        break;
                    case DialogResult.No: e.Cancel = false;
                        break;
                    case DialogResult.Cancel: e.Cancel = true;
                        break;
                }
            if (!e.Cancel)
                try
                {
                    Directory.Delete(TempDirectory, true);
                }
                catch
                {
                    MessageBox.Show("Ошибка при удалении каталога хранения временных файлов");
                }
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (SelectedObjectIdx < 0)
                {
                    MessageBox.Show("Выберите объект двойным щелчком мыши по нему.");
                    return;
                }

                timerDraw.Enabled = false;

                UnitCount--;
                model[SelectedObjectIdx] = model[UnitCount];
                model[UnitCount] = null;

                #region Модификации модели
                MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount] = MeshMod[UnitCount - CoordLineCount - SelectionLineCount];
                MeshMod[UnitCount - CoordLineCount - SelectionLineCount] = null;
                listModifers.Items.Clear();
                tabModProperties.Visible = (listModifers.Items.Count > 0);
                #endregion

                MeshListUpdate();

                SelectedObjectIdx = -1;
                DeselectObject();

                #region Инициализация буфера сортировки моделей
                ActiveUnitIndex = new int[UnitCount];
                #endregion

                #region Инициализация контроллера моделей
                for (int i = 0; i < UnitCount + 1; i++)
                    if (model[i] != null) modelController.Collection[i] = model[i]; else modelController.Collection[i] = null;
                if (modelController.CreateActivePolygonBuffer() != 0) throw new Exception(ErrorLog.GetLastError());
                #endregion

                isEditedProject = true;
                isEditedObject = true;

                timerDraw.Enabled = true;
            }
            catch (Exception er)
            {
                MessageBox.Show("удалитьToolStripMenuItem_Click\n" + er.Message);
            }
        }

        private void pictMain_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                switch (e.Button)
                {
                    case (System.Windows.Forms.MouseButtons.Left):
                        {
                            RollingCamera = true;
                            MouseLeftX = e.X;
                            MouseLeftY = e.Y;
                        }
                        break;
                    case (System.Windows.Forms.MouseButtons.Right):
                        {
                            MovingCamera = true;
                            MouseRightX = e.X;
                            MouseRightY = e.Y;
                        }
                        break;
                    case (System.Windows.Forms.MouseButtons.Middle):
                        {
                            BankCamera = true;
                            MouseCenterX = e.X;
                            MouseCenterY = e.Y;
                        }
                        break;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show("pictMain_MouseDown\n" + er.Message);
            }
        }

        private void pictMain_MouseWeel(object sender, MouseEventArgs e)
        {
            CameraPos.Z += (float)e.Delta * MouseWheelDiv / 100 * (MouseWheelInverse ? -1 : 1);
        }

        private void pictMain_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                switch (e.Button)
                {
                    case (System.Windows.Forms.MouseButtons.Left):
                        RollingCamera = false;
                        break;
                    case (System.Windows.Forms.MouseButtons.Right):
                        MovingCamera = false;
                        break;
                    case (System.Windows.Forms.MouseButtons.Middle):
                        BankCamera = false;
                        break;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show("pictMain_MouseUp\n" + er.Message);
            }
        }

        private void pictMain_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (MovingCamera)
                {


                    CameraAngle.Z += Engine3D.RadianDegrees * (float)(e.X - MouseRightX) * MouseAxisDiv / 1000f;
                    CameraAngle.Y += Engine3D.RadianDegrees * (float)(e.Y - MouseRightY) * MouseAxisDiv / 1000f;

                    /*
                    Engine3D.RotatePoint3D((-MouseRightY + e.Y) * Engine3D.RadianDegrees / 10, Axis3D.OXyz, CameraAngle);
                    Engine3D.RotatePoint3D((-MouseRightX + e.X) * Engine3D.RadianDegrees / 10, Axis3D.OYxz, CameraAngle);
                    */

                    //if (CameraAngle.Y > Engine3D.Radian90) CameraAngle.Y = Engine3D.Radian90;
                    //if (CameraAngle.Y < -Engine3D.Radian90) CameraAngle.Y = -Engine3D.Radian90;

                    MouseRightX = e.X;
                    MouseRightY = e.Y;
                }
                if (RollingCamera)
                {

                    CameraPos.Y += (float)(MouseLeftY - e.Y) * MouseXYDiv / 100;

                    /*
                    Engine3D.RotatePoint3D((-MouseLeftY + e.Y) * Engine3D.RadianDegrees/10,Axis3D.OXyz,CameraPos);
                    Engine3D.RotatePoint3D((-MouseLeftX + e.X) * Engine3D.RadianDegrees/10, Axis3D.OYxz, CameraPos);
                    */

                    MouseLeftY = e.Y;

                    CameraPos.X += (float)(e.X - MouseLeftX) * MouseXYDiv / 100;
                    MouseLeftX = e.X;
                }

                if (BankCamera)
                {
                    CameraAngle.X += Engine3D.RadianDegrees * (float)(e.X - MouseCenterX) * MouseAxisDiv / 1000f;
                    CameraAngle.Y += Engine3D.RadianDegrees * (float)(e.Y - MouseCenterY) * MouseAxisDiv / 1000f;

                    MouseCenterX = e.X;
                    MouseCenterY = e.Y;
                }

            }
            catch (Exception er)
            {
                MessageBox.Show("pictMain_MouseMove\n" + er.Message);
            }
        }

        /// <summary>
        /// Создание новой модели
        /// </summary>
        /// <param name="modelType"></param>
        /// <param name="FileName"></param>
        public void AddObject(ModelTypes modelType)
        {
            try
            {
                if (UnitCount < (MaxUnitCount - 1))
                {
                    #region Добавляем юнит

                    UnitCount++;

                    switch (modelType)
                    {
                        case ModelTypes.Plane3D:
                            model[UnitCount - 1] = new Plane3D(9, 50, 50, 50, 50);
                            break;
                        case ModelTypes.CellPlane3D:
                            model[UnitCount - 1] = new CellPlane3D(5, 5, 100, 100);
                            break;
                        case ModelTypes.Cylinder3D:
                            model[UnitCount - 1] = new Cylinder3D(9, 2, 100, 50, 50, 50, 50, 50, 50, 50, 50);
                            break;
                        case ModelTypes.Ellipse3D:
                            model[UnitCount - 1] = new Ellipse3D(9, 9, 50, 50, 50, 50, 50, 50);
                            break;
                        case ModelTypes.Prism3D:
                            model[UnitCount - 1] = new Prism3D(5, 5, 5);
                            break;
                        case ModelTypes.Tor3D:
                            model[UnitCount - 1] = new Tor3D(9, 5, 40, 40, 40, 40, 10, 10, 10, 10, 10, 10
                                , Engine3D.RadianDegrees * 90, Engine3D.RadianDegrees * 90 + Engine3D.Radian360
                                , Engine3D.RadianDegrees * 90, Engine3D.RadianDegrees * 90 + Engine3D.Radian360
                                );
                            break;
                        case ModelTypes.SurfaceHole3D:
                            model[UnitCount - 1] = new SurfaceHole3D(5, 5, 50, 50, 50, 50);
                            break;
                        case ModelTypes.ComplexModel3D:
                            model[UnitCount - 1] = new ComplexModel3D();
                            break;
                    }

                    if (modelType != ModelTypes.ComplexModel3D)
                    {
                        model[UnitCount - 1].CreatePolygonMap();

                        switch (modelType)
                        {
                            case ModelTypes.Plane3D:
                                model[UnitCount - 1].SetDoubleSided(true);
                                break;
                            case ModelTypes.CellPlane3D:
                                model[UnitCount - 1].SetDoubleSided(true);
                                break;
                            case ModelTypes.SurfaceHole3D:
                                model[UnitCount - 1].SetDoubleSided(true);
                                break;
                        }

                        model[UnitCount - 1].SetFillType(PolygonFillType.Solid);
                        model[UnitCount - 1].SetColor(Color.DarkRed, 0, -1, PolygonSides.FrontSide);
                        model[UnitCount - 1].SetColor(Color.Gray, 0, -1, PolygonSides.RearSide);
                        model[UnitCount - 1].SetMatte((float)0.5, 0, -1, PolygonSides.FrontSide);
                    }
                    #endregion

                    #region Модификации модели
                    MeshMod[UnitCount - 1 - CoordLineCount - SelectionLineCount] = new MeshModifications(MaxModifyCount);
                    listModifers.Items.Clear();
                    tabModProperties.Visible = false;
                    #endregion

                    #region Инициализация буфера сортировки моделей
                    ActiveUnitIndex = new int[UnitCount];
                    #endregion

                    #region Инициализация контроллера моделей
                    for (int i = 0; i < UnitCount; i++)
                        if (model[i] != null) modelController.Collection[i] = model[i];
                    if (modelController.CreateActivePolygonBuffer() != 0) throw new Exception(ErrorLog.GetLastError());
                    #endregion

                    MeshListUpdate();

                    MouseSelectedObjectIdx = UnitCount - 1;
                    MouseSelectObject();

                    isEditedProject = true;
                    isEditedObject = true;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show("AddObject\n" + er.Message);
            }
        }

        /// <summary>
        /// Загрузка модели из файла
        /// </summary>
        /// <param name="modelType"></param>
        /// <param name="FileName"></param>
        public void AddObject(string FileName = "")
        {
            try
            {
                VisualComponentMethodLock++;

                ModelTypes modelType = ModelTypes.NONE;

                if (UnitCount < (MaxUnitCount - 1))
                {
                    UnitCount++;


                    #region Определяем тип модели
                    {
                        CultureInfo culture = CultureInfo.InvariantCulture;
                        StreamReader file = new StreamReader(FileName, System.Text.Encoding.Default);
                        try
                        {
                            string sLine = "";
                            int currentMode = -1;

                            while (!file.EndOfStream)
                            {
                                sLine = file.ReadLine().Trim();
                                if (sLine == "") continue;
                                if (sLine.Substring(0, 1) == "*") continue;

                                switch (sLine)
                                {
                                    case "#ModelType#":
                                        currentMode = 0;
                                        continue;
                                    default:
                                        if (sLine.Substring(0, 1) == "#")
                                        {
                                            currentMode = -1;
                                            continue;
                                        }
                                        break;
                                }
                                if (currentMode == -1) continue;

                                modelType = (ModelTypes)int.Parse(sLine);
                                break;
                            }
                        }
                        finally
                        {
                            file.Close();
                            file.Dispose();
                        }
                    }
                    #endregion

                    #region Загружаем модель
                    switch (modelType)
                    {
                        case ModelTypes.Plane3D:
                            model[UnitCount - 1] = new Plane3D(FileName);
                            break;
                        case ModelTypes.CellPlane3D:
                            model[UnitCount - 1] = new CellPlane3D(FileName);
                            break;
                        case ModelTypes.Cylinder3D:
                            model[UnitCount - 1] = new Cylinder3D(FileName);
                            break;
                        case ModelTypes.Ellipse3D:
                            model[UnitCount - 1] = new Ellipse3D(FileName);
                            break;
                        case ModelTypes.Prism3D:
                            model[UnitCount - 1] = new Prism3D(FileName);
                            break;
                        case ModelTypes.Tor3D:
                            model[UnitCount - 1] = new Tor3D(FileName);
                            break;
                        case ModelTypes.SurfaceHole3D:
                            model[UnitCount - 1] = new SurfaceHole3D(FileName);
                            break;
                        case ModelTypes.ComplexModel3D:
                            model[UnitCount - 1] = new ComplexModel3D(FileName);
                            break;
                    }
                    #endregion

                    #region Модификации модели
                    MeshMod[UnitCount - 1 - CoordLineCount - SelectionLineCount] = new MeshModifications(MaxModifyCount);
                    listModifers.Items.Clear();
                    tabModProperties.Visible = false;
                    MeshMod[UnitCount - 1 - CoordLineCount - SelectionLineCount].LoadFromFile(FileName);

                    #region Определяем шаблон файла для i-го объекта               
                    int pos = FileName.LastIndexOf(".");
                    if (pos >= 0) FileName = FileName.Substring(0, pos).Trim();
                    if (FileName == "") throw new Exception("Некорректное имя файла");
                    FileName += ".mdy";
                    #endregion
                    MeshMod[UnitCount - 1 - CoordLineCount - SelectionLineCount].LoadFromFile(FileName, true);

                    MeshMod[UnitCount - 1 - CoordLineCount - SelectionLineCount].RefreshList(listModifers);
                    #endregion



                    #region Инициализация буфера сортировки моделей
                    ActiveUnitIndex = new int[UnitCount];
                    #endregion

                    #region Инициализация контроллера моделей
                    for (int i = 0; i < UnitCount; i++)
                        if (model[i] != null) modelController.Collection[i] = model[i];
                    if (modelController.CreateActivePolygonBuffer() != 0) throw new Exception(ErrorLog.GetLastError());
                    #endregion

                    MeshListUpdate();

                    MouseSelectedObjectIdx = UnitCount - 1;
                    MouseSelectObject();

                    isEditedProject = true;
                    isEditedObject = true;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show("AddObject\n" + er.Message);
            }
            VisualComponentMethodLock--;
        }

        /// <summary>
        /// копирование модели
        /// </summary>
        /// <param name="index"></param>
        public void AddObject(int index)
        {
            try
            {
                if (UnitCount < (MaxUnitCount - 1))
                {
                    #region Добавляем юнит

                    UnitCount++;

                    switch (model[index].ModelType())
                    {
                        case ModelTypes.Plane3D:
                            model[UnitCount - 1] = new Plane3D((Plane3D)model[index]);
                            break;
                        case ModelTypes.CellPlane3D:
                            model[UnitCount - 1] = new CellPlane3D((CellPlane3D)model[index]);
                            break;
                        case ModelTypes.Cylinder3D:
                            model[UnitCount - 1] = new Cylinder3D((Cylinder3D)model[index]);
                            break;
                        case ModelTypes.Ellipse3D:
                            model[UnitCount - 1] = new Ellipse3D((Ellipse3D)model[index]);
                            break;
                        case ModelTypes.Prism3D:
                            model[UnitCount - 1] = new Prism3D((Prism3D)model[index]);
                            break;
                        case ModelTypes.Tor3D:
                            model[UnitCount - 1] = new Tor3D((Tor3D)model[index]);
                            break;
                        case ModelTypes.SurfaceHole3D:
                            model[UnitCount - 1] = new SurfaceHole3D((SurfaceHole3D)model[index]);
                            break;
                        case ModelTypes.ComplexModel3D:
                            model[UnitCount - 1] = new ComplexModel3D(model[index]);
                            break;
                    }

                    #endregion

                    #region Модификации модели
                    int newIdx = UnitCount - 1 - CoordLineCount - SelectionLineCount;
                    MeshMod[newIdx] = new MeshModifications(MaxModifyCount);
                    for (int i = 0; i < MeshMod[index - CoordLineCount - SelectionLineCount].CurrentLength; i++) MeshMod[newIdx].AddCopy(MeshMod[index - CoordLineCount - SelectionLineCount].MeshInfo[i]);
                    listModifers.Items.Clear();
                    tabModProperties.Visible = false;
                    MeshMod[newIdx].RefreshList(listModifers);
                    #endregion

                    #region Инициализация буфера сортировки моделей
                    ActiveUnitIndex = new int[UnitCount];
                    #endregion

                    #region Инициализация контроллера моделей
                    for (int i = 0; i < UnitCount; i++)
                        if (model[i] != null) modelController.Collection[i] = model[i];
                    if (modelController.CreateActivePolygonBuffer() != 0) throw new Exception(ErrorLog.GetLastError());
                    #endregion

                    MeshListUpdate();

                    MouseSelectedObjectIdx = UnitCount - 1;
                    MouseSelectObject();

                    isEditedProject = true;
                    isEditedObject = true;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show("AddObject-2\n" + er.Message);
            }
        }

        private void MeshListUpdate()
        {
            try
            {
                VisualComponentMethodLock++;
                cmbMesh.Items.Clear();
                for (int i = CoordLineCount + SelectionLineCount; i < UnitCount; i++) cmbMesh.Items.Add("№ " + (i - CoordLineCount - SelectionLineCount).ToString() + " - " + model[i].ModelType().ToString());
                VisualComponentMethodLock--;
            }
            catch (Exception er)
            {
                MessageBox.Show("MeshListUpdate\n" + er.Message);
            }
        }

        private void плоскийЭллипсToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timerDraw.Enabled = false;
            AddObject(ModelTypes.Plane3D);
            timerDraw.Enabled = true;
        }

        private void прямоугольнаяПоверхностьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timerDraw.Enabled = false;
            AddObject(ModelTypes.CellPlane3D);
            timerDraw.Enabled = true;
        }

        private void конусToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void цилиндрToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timerDraw.Enabled = false;
            AddObject(ModelTypes.Cylinder3D);
            timerDraw.Enabled = true;
        }

        private void эллипсоидToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timerDraw.Enabled = false;
            AddObject(ModelTypes.Ellipse3D);
            timerDraw.Enabled = true;
        }

        private void торToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timerDraw.Enabled = false;
            AddObject(ModelTypes.Tor3D);
            timerDraw.Enabled = true;
        }

        private void многосоставнаяПризмаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timerDraw.Enabled = false;
            AddObject(ModelTypes.Prism3D);
            timerDraw.Enabled = true;
        }

        private void полиномToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void готоваяМодельИзФайлаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timerDraw.Enabled = false;
            try
            {
                #region Запрашиваем имя файла
                if (openObjectDlg.ShowDialog() != DialogResult.OK)
                {
                    timerDraw.Enabled = true;
                    return;
                }
                #endregion

                AddObject(openObjectDlg.FileName);

            }
            catch (Exception er)
            {
                MessageBox.Show("готоваяМодельИзФайлаToolStripMenuItem_Click\n" + er.Message);
            }
            timerDraw.Enabled = true;
        }

        private void pictMain_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                timerDraw.Enabled = false;
                MouseSelectPos.X = MouseLeftX;
                MouseSelectPos.Y = MouseLeftY;
                MouseSelectedObjectIdx = -1;
                DeselectObject();
                timerDraw.Enabled = true;
            }
            catch (Exception er)
            {
                MessageBox.Show("pictMain_DoubleClick\n" + er.Message);
            }
        }



        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            CameraPos.X = 0;
            CameraPos.Y = 0;
            CameraPos.Z = -CameraPosZ;
        }

        private void видСпереди0XYToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CameraAngle.X = Engine3D.RadianDegrees * 0;
            CameraAngle.Y = Engine3D.RadianDegrees * 0;
            CameraAngle.Z = Engine3D.RadianDegrees * 0;
        }

        private void видСзади0XYToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CameraAngle.X = Engine3D.RadianDegrees * 0;
            CameraAngle.Y = Engine3D.RadianDegrees * 0;
            CameraAngle.Z = Engine3D.RadianDegrees * 180;
        }

        private void видСлева0YZToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CameraAngle.X = Engine3D.RadianDegrees * 0;
            CameraAngle.Y = Engine3D.RadianDegrees * 0;
            CameraAngle.Z = Engine3D.RadianDegrees * 90;
        }

        private void видСправа0YZToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CameraAngle.X = Engine3D.RadianDegrees * 0;
            CameraAngle.Y = Engine3D.RadianDegrees * 0;
            CameraAngle.Z = Engine3D.RadianDegrees * -90;
        }

        private void видСверху0XZToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CameraAngle.X = Engine3D.RadianDegrees * 0;
            CameraAngle.Y = Engine3D.RadianDegrees * 90;
            CameraAngle.Z = Engine3D.RadianDegrees * 0;
        }

        private void видСнизу0XZToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CameraAngle.X = Engine3D.RadianDegrees * 0;
            CameraAngle.Y = Engine3D.RadianDegrees * -90;
            CameraAngle.Z = Engine3D.RadianDegrees * 0;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnColorDialog_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void nudMatte1_ValueChanged(object sender, EventArgs e)
        {
        }

        private void nudSizeX_ValueChanged(object sender, EventArgs e)
        {
        }

        private void nudPosX_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13) e.SuppressKeyPress = true;
        }

        private void nudSizeX_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13) e.SuppressKeyPress = true;
        }

        private void nudMatte1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13) e.SuppressKeyPress = true;
        }


        private void слияниеОбъектовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if ((CoordLineCount + SelectionLineCount) == UnitCount)
                {
                    MessageBox.Show("Проект не содержит моделей");
                    return;
                }
                if (MessageBox.Show("Выполнить слияние всех видимых объектов?", "Внимание", MessageBoxButtons.OKCancel) != DialogResult.OK) return;

                timerDraw.Enabled = false;
                Application.DoEvents();

                #region Применение модификаций
                for (int i = CoordLineCount + SelectionLineCount; i < UnitCount; i++)
                {
                    model[i].ResetCameraModel();
                    ModifyModel(model[i], MeshMod[i - CoordLineCount - SelectionLineCount]);
                    model[i].SaveCameraModelToMain();
                    MeshMod[i - CoordLineCount - SelectionLineCount].Clear();
                }
                #endregion

                #region Создание новой модели и слияние её с всеми существующими
                ComplexModel3D temp = new ComplexModel3D(model[CoordLineCount + SelectionLineCount]);
                for (int i = CoordLineCount + SelectionLineCount + 1; i < UnitCount; i++)
                {
                    if (temp.MergeObject(model[i]) != 0) throw new Exception(ErrorLog.GetLastError());
                    model[i] = null;
                }
                if (temp.ResetActivePolygonIndexes() != 0) throw new Exception(ErrorLog.GetLastError());
                model[CoordLineCount + SelectionLineCount] = temp;
                #endregion

                UnitCount = CoordLineCount + SelectionLineCount + 1;
                MeshListUpdate();
                SelectedObjectIdx = -2;
                DeselectObject();

                #region Инициализация буфера сортировки моделей
                ActiveUnitIndex = new int[UnitCount];
                #endregion

                InitModelController();
                MouseSelectedObjectIdx = UnitCount - 1;
                MouseSelectObject();

                isEditedProject = true;
                isEditedObject = true;

                timerDraw.Enabled = true;

            }
            catch (Exception er)
            {
                MessageBox.Show("слияниеОбъектовToolStripMenuItem_Click\n" + er.Message);
            }
        }

        public void InitModelController()
        {
            #region Инициализация контроллера моделей
            for (int i = 0; i < modelController.Collection.Length; i++) modelController.Collection[i] = null;
            for (int i = 0; i < UnitCount; i++)
                if (model[i] != null) modelController.Collection[i] = model[i];
            if (modelController.CreateActivePolygonBuffer() != 0) throw new Exception(ErrorLog.GetLastError());
            #endregion
        }

        private void cmbMesh_Click(object sender, EventArgs e)
        {

        }

        private void cmbMesh_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (VisualComponentMethodLock > 0) return;
                MouseSelectedObjectIdx = cmbMesh.SelectedIndex + CoordLineCount + SelectionLineCount;
                MouseSelectObject();
            }
            catch (Exception er)
            {
                MessageBox.Show("cmbMesh_SelectedIndexChanged\n" + er.Message);
            }
        }


        private void cmbFillType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (VisualComponentMethodLock > 0) return;
                model[SelectedObjectIdx].SetFillType((cmbFillType.SelectedIndex == 0) ? PolygonFillType.Solid : PolygonFillType.Wide, 0, -1);
                isEditedProject = true;
                isEditedObject = true;
            }
            catch (Exception er)
            {
                MessageBox.Show("cmbFillType_SelectedIndexChanged\n" + er.Message);
            }
        }

        private void chkDoubleSided_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (VisualComponentMethodLock > 0) return;
                model[SelectedObjectIdx].SetDoubleSided(chkDoubleSided.Checked, 0, -1);
                isEditedProject = true;
                isEditedObject = true;
            }
            catch (Exception er)
            {
                MessageBox.Show("chkDoubleSided_CheckedChanged\n" + er.Message);
            }
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (saveProjectDlg.FileName == "") if (saveProjectDlg.ShowDialog() != DialogResult.OK) return;

                #region Сохраняем проект
                SaveProject(saveProjectDlg.FileName);
                isEditedProject = false;
                isEditedObject = false;
                this.Text = "Студия трехмерных моделей - " + saveProjectDlg.FileName;
                #endregion
            }
            catch (Exception er)
            {
                MessageBox.Show("сохранитьToolStripMenuItem_Click\n" + er.Message);
            }
        }

        private void копироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (SelectedObjectIdx < 0) throw new Exception("Следует сначала выбрать образец для копирования");
                AddObject(SelectedObjectIdx);
            }
            catch (Exception er)
            {
                MessageBox.Show("копироватьToolStripMenuItem_Click\n" + er.Message);
            }
        }


        private void освещениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                frmLightDialog frm = new frmLightDialog();
                frm.panelAmbient.BackColor = Color.FromArgb(255, AmbientColorR, AmbientColorG, AmbientColorB);
                frm.trackAmbient.Value = (int)Math.Round(AmbientLightPower * 100);
                frm.trackAmbient_ValueChanged(null, null);

                frm.panelDirect.BackColor = DirectLightColor[0];
                frm.trackDirect.Value = (int)Math.Round(DirectLightPower[0] * 100);
                frm.trackDirect_ValueChanged(null, null);

                frm.panelBackground.BackColor = Color.FromArgb(255, BackgroundColorR, BackgroundColorG, BackgroundColorB);

                frm.ShowDialog();

                if (frm.dialogResult == DialogResult.OK)
                {
                    AmbientColorR = frm.panelAmbient.BackColor.R;
                    AmbientColorG = frm.panelAmbient.BackColor.G;
                    AmbientColorB = frm.panelAmbient.BackColor.B;
                    AmbientLightPower = ((float)frm.trackAmbient.Value) / 100;

                    DirectLightColor[0] = frm.panelDirect.BackColor;
                    DirectLightPower[0] = ((float)frm.trackDirect.Value / 100);

                    BackgroundColorR = frm.panelBackground.BackColor.R;
                    BackgroundColorG = frm.panelBackground.BackColor.G;
                    BackgroundColorB = frm.panelBackground.BackColor.B;
                }

                frm.Dispose();
            }
            catch (Exception er)
            {
                MessageBox.Show("освещениеToolStripMenuItem_Click\n" + er.Message);
            }
        }

        private void panelColor1_Click(object sender, EventArgs e)
        {
            try
            {
                if (lblColor1.Visible) return;
                colorDialog1.Color = panelColor1.BackColor;
                if (colorDialog1.ShowDialog() != DialogResult.OK) return;
                panelColor1.BackColor = colorDialog1.Color;
                //if (VisualComponentMethodLock > 0) return;
                model[SelectedObjectIdx].SetColor(panelColor1.BackColor, 0, -1, PolygonSides.FrontSide);
                trackOpacity_ValueChanged(null, null);

                VisualComponentMethodLock++;
                nudColorExtCode.Value = panelColor1.BackColor.ToArgb();
                VisualComponentMethodLock--;

                isEditedProject = true;
                isEditedObject = true;
            }
            catch (Exception er)
            {
                MessageBox.Show("panelColor1_Click\n" + er.Message);
            }
        }

        private void panelColor2_Click(object sender, EventArgs e)
        {
            try
            {
                if (lblColor2.Visible) return;
                colorDialog1.Color = panelColor2.BackColor;
                if (colorDialog1.ShowDialog() != DialogResult.OK) return;
                panelColor2.BackColor = colorDialog1.Color;
                //if (VisualComponentMethodLock > 0) return;
                model[SelectedObjectIdx].SetColor(panelColor2.BackColor, 0, -1, PolygonSides.RearSide);
                trackOpacity_ValueChanged(null, null);

                VisualComponentMethodLock++;
                nudColorIntCode.Value = panelColor2.BackColor.ToArgb();
                VisualComponentMethodLock--;

                isEditedProject = true;
                isEditedObject = true;
            }
            catch (Exception er)
            {
                MessageBox.Show("panelColor2_Click\n" + er.Message);
            }
        }

        private void изометрическийВидToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CameraAngle.X = Engine3D.RadianDegrees * 0;
            CameraAngle.Y = Engine3D.RadianDegrees * 30;
            CameraAngle.Z = Engine3D.RadianDegrees * 225;
        }

        private void trackOpacity_ValueChanged(object sender, EventArgs e)
        {
            lblOpacityVal.Text = "Непрозрачность: " + trackOpacity.Value.ToString() + "%";
            if (VisualComponentMethodLock > 0) return;
            for (int i = 0; i < model[SelectedObjectIdx].Polygon.Length; i++)
            {
                model[SelectedObjectIdx].SetColor(Color.FromArgb((int)Math.Round((float)trackOpacity.Value * 255 / 100), model[SelectedObjectIdx].Polygon[i].color[0]), i, i, PolygonSides.FrontSide);
                model[SelectedObjectIdx].SetColor(Color.FromArgb((int)Math.Round((float)trackOpacity.Value * 255 / 100), model[SelectedObjectIdx].Polygon[i].color[1]), i, i, PolygonSides.RearSide);
            }
            isEditedProject = true;
            isEditedObject = true;
        }

        private void chkClosedSurface_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (VisualComponentMethodLock > 0) return;
                model[SelectedObjectIdx].ClosedSurface = chkClosedSurface.Checked;
                isEditedProject = true;
                isEditedObject = true;
            }
            catch (Exception er)
            {
                MessageBox.Show("chkClosedSurface_CheckedChanged\n" + er.Message);
            }
        }

        private void nudSettingN1_ValueChanged(object sender, EventArgs e)
        {
            SetGeometryParameters();
        }

        private void nudSettingN2_ValueChanged(object sender, EventArgs e)
        {
            SetGeometryParameters();

        }

        private void nudSettingN3_ValueChanged(object sender, EventArgs e)
        {
            SetGeometryParameters();
        }

        private void btnCenter_Click(object sender, EventArgs e)
        {

        }

        private void nudAngle1Start_ValueChanged(object sender, EventArgs e)
        {
            SetGeometryParameters();
        }

        private void btnRotateCenter_Click(object sender, EventArgs e)
        {

        }

        private void nudRotateOX_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btnRotate_Click(object sender, EventArgs e)
        {

        }

        private void chkBottom_CheckedChanged(object sender, EventArgs e)
        {
            SetGeometryParameters();
        }

        private void NudR1Xplus_ValueChanged(object sender, EventArgs e)
        {
            SetGeometryParameters();
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            try
            {
                if (SelectedObjectIdx < 0)
                {
                    MessageBox.Show("Выберите объект двойным щелчком мыши по нему.");
                    return;
                }
                VisualComponentMethodLock++;
                MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].Add(new MeshModify(0));
                MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].RefreshList(listModifers);
                tabModProperties.SelectedIndex = 0;
                listModifers.Focus();
                listModifers.SelectedIndex = listModifers.Items.Count - 1;
                tabModProperties.Visible = true;
                VisualComponentMethodLock--;
                listModifers_SelectedIndexChanged(null, null);
                isEditedProject = true;
                isEditedObject = true;
            }
            catch (Exception er)
            {
                MessageBox.Show("toolStripButton7_Click\n" + er.Message);
                VisualComponentMethodLock--;
            }
        }

        private void tabModProperties_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (VisualComponentMethodLock > 0) return;
                if (SelectedObjectIdx < 0) return;
                if (listModifers.SelectedIndex < 0) return;

                MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].SetCode(tabModProperties.SelectedIndex);
                MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].UpdateListBox(listModifers, listModifers.SelectedIndex);

                isEditedProject = true;
                isEditedObject = true;
            }
            catch (Exception er)
            {
                MessageBox.Show("tabModProperties_SelectedIndexChanged\n" + er.Message);
            }
        }

        private void toolStripButton13_Click(object sender, EventArgs e)
        {
            try
            {
                if (SelectedObjectIdx < 0)
                {
                    MessageBox.Show("Выберите объект двойным щелчком мыши по нему.");
                    return;
                }
                if (listModifers.SelectedIndex < 0) return;
                VisualComponentMethodLock++;
                MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].Remove(listModifers.SelectedIndex);
                MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].RefreshList(listModifers);
                listModifers.SelectedIndex = (listModifers.Items.Count > 0) ? 0 : -1;
                tabModProperties.Visible = (listModifers.Items.Count > 0);
                if (tabModProperties.Visible) tabModProperties.SelectedIndex = MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[0].ModifyCode;
                VisualComponentMethodLock--;

                isEditedProject = true;
                isEditedObject = true;
            }
            catch (Exception er)
            {
                MessageBox.Show("toolStripButton7_Click\n" + er.Message);
                VisualComponentMethodLock--;
            }
        }

        private void listModifers_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //if (VisualComponentMethodLock > 0) return;
                if (listModifers.SelectedIndex < 0) return;
                VisualComponentMethodLock++;
                tabModProperties.Visible = true;
                tabModProperties.SelectedIndex = MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].ModifyCode;
                switch (tabModProperties.SelectedIndex)
                {
                    case 0:
                        nudMoveDX.Value = (decimal)MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].floatParameters[0];
                        nudMoveDY.Value = (decimal)MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].floatParameters[1];
                        nudMoveDZ.Value = (decimal)MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].floatParameters[2];
                        break;
                    case 1:
                        nudScaleDX.Value = (decimal)MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].floatParameters[0];
                        nudScaleDY.Value = (decimal)MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].floatParameters[1];
                        nudScaleDZ.Value = (decimal)MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].floatParameters[2];
                        break;
                    case 2:
                        cmbRotateAxis.SelectedIndex = MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].intParameters[0];
                        nudRotateAxis.Value = (decimal)(MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].floatParameters[0] / Engine3D.RadianDegrees);
                        break;
                    case 3:
                        switch (MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].intParameters[0])
                        {
                            case 0:
                                rbCenter.Checked = true;
                                break;
                            case 1:
                                rbXZ.Checked = true;
                                break;
                            case 2:
                                rbYZ.Checked = true;
                                break;
                            case 3:
                                rbXY.Checked = true;
                                break;
                        }
                        break;
                    case 4:
                        nudTwistAngleBottom.Value = (decimal)(MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].floatParameters[0] / Engine3D.RadianDegrees);
                        nudTwistAngleTop.Value = (decimal)(MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].floatParameters[1] / Engine3D.RadianDegrees);
                        nudTwistLevelBottom.Value = (decimal)(MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].floatParameters[2]);
                        nudTwistLevelTop.Value = (decimal)(MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].floatParameters[3]);
                        checkTwistDown.Checked = (MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].intParameters[0] == 1);
                        checkTwistTop.Checked = (MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].intParameters[1] == 1);
                        break;
                    case 5:
                        nudBendAngle.Value = (decimal)(MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].floatParameters[0] / Engine3D.RadianDegrees);
                        nudBendBottom.Value = (decimal)(MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].floatParameters[1]);
                        nudBendTop.Value = (decimal)(MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].floatParameters[2]);
                        break;
                    case 6:
                        nudCollapseXmin.Value = (decimal)MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].floatParameters[0];
                        nudCollapseXmax.Value = (decimal)MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].floatParameters[1];
                        nudCollapseYmin.Value = (decimal)MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].floatParameters[2];
                        nudCollapseYmax.Value = (decimal)MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].floatParameters[3];
                        nudCollapseZmin.Value = (decimal)MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].floatParameters[4];
                        nudCollapseZmax.Value = (decimal)MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].floatParameters[5];
                        nudCollapseDY1.Value = (decimal)MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].floatParameters[6];
                        nudCollapseDY2.Value = (decimal)MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].floatParameters[7];
                        break;
                        /*
                    case 7:
                        NudGrdSphX.Value = (decimal)MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].floatParameters[0];
                        NudGrdSphY.Value = (decimal)MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].floatParameters[1];
                        NudGrdSphZ.Value = (decimal)MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].floatParameters[2];
                        NudGrdSphRin.Value = (decimal)MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].floatParameters[3];
                        NudGrdSphR.Value = (decimal)MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].floatParameters[4];
                        nudGrdSphTransparentIn.Value = (decimal)MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].floatParameters[5];
                        nudGrdSphTransparentOut.Value = (decimal)MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].floatParameters[6];
                        nudGrdSphTransparentTotal.Value = (decimal)MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].floatParameters[7];

                        checkGrdSphInColor.Checked = (MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].intParameters[0] == 1);
                        checkGrdSphOutColor.Checked = (MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].intParameters[1] == 1);

                        panelGrdSphColorIn.BackColor = Color.FromArgb(255, 
                            MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].intParameters[2],
                            MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].intParameters[3],
                            MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].intParameters[4]);

                        panelGrdSphColorOut.BackColor = Color.FromArgb(255,
                            MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].intParameters[5],
                            MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].intParameters[6],
                            MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].intParameters[7]);

                        cmbGrdSphSide.SelectedIndex = MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].intParameters[8];
                        break;
                        */

                }
                VisualComponentMethodLock--;
            }
            catch (Exception er)
            {
                MessageBox.Show("listModifers_SelectedIndexChanged\n" + er.Message);
                VisualComponentMethodLock--;
            }
        }

        private void nudMove_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (VisualComponentMethodLock > 0) return;
                if (SelectedObjectIdx < 0) return;
                if (listModifers.SelectedIndex < 0) return;
                MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].floatParameters[0] = (float)nudMoveDX.Value;
                MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].floatParameters[1] = (float)nudMoveDY.Value;
                MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].floatParameters[2] = (float)nudMoveDZ.Value;
                MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].UpdateListBox(listModifers, listModifers.SelectedIndex);
                isEditedProject = true;
                isEditedObject = true;
            }
            catch (Exception er)
            {
                MessageBox.Show("nudMove_ValueChanged\n" + er.Message);
            }
        }

        private void nudScale_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (VisualComponentMethodLock > 0) return;
                if (SelectedObjectIdx < 0) return;
                if (listModifers.SelectedIndex < 0) return;
                VisualComponentMethodLock++;
                if (chkScaleAll.Checked)
                {
                    if (((NumericUpDown)sender).Name == "nudScaleDX") nudScaleDY.Value = nudScaleDZ.Value = nudScaleDX.Value;
                    if (((NumericUpDown)sender).Name == "nudScaleDY") nudScaleDX.Value = nudScaleDZ.Value = nudScaleDY.Value;
                    if (((NumericUpDown)sender).Name == "nudScaleDZ") nudScaleDY.Value = nudScaleDX.Value = nudScaleDZ.Value;
                }
                MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].floatParameters[0] = (float)nudScaleDX.Value;
                MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].floatParameters[1] = (float)nudScaleDY.Value;
                MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].floatParameters[2] = (float)nudScaleDZ.Value;
                MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].UpdateListBox(listModifers, listModifers.SelectedIndex);
                VisualComponentMethodLock--;
                isEditedProject = true;
                isEditedObject = true;
            }
            catch (Exception er)
            {
                MessageBox.Show("nudScale_ValueChanged\n" + er.Message);
            }
        }

        private void cmbRotateAxis_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (VisualComponentMethodLock > 0) return;
                if (SelectedObjectIdx < 0) return;
                if (listModifers.SelectedIndex < 0) return;
                VisualComponentMethodLock++;
                MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].intParameters[0] = cmbRotateAxis.SelectedIndex;
                MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].UpdateListBox(listModifers, listModifers.SelectedIndex);
                VisualComponentMethodLock--;
                isEditedProject = true;
                isEditedObject = true;
            }
            catch (Exception er)
            {
                MessageBox.Show("cmbRotateAxis_SelectedIndexChanged\n" + er.Message);
            }
        }

        private void nudRotateAxis_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (VisualComponentMethodLock > 0) return;
                if (SelectedObjectIdx < 0) return;
                if (listModifers.SelectedIndex < 0) return;
                VisualComponentMethodLock++;
                MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].floatParameters[0] = (float)nudRotateAxis.Value * Engine3D.RadianDegrees;
                MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].UpdateListBox(listModifers, listModifers.SelectedIndex);
                VisualComponentMethodLock--;
                isEditedProject = true;
                isEditedObject = true;
            }
            catch (Exception er)
            {
                MessageBox.Show("nudRotateAxis_ValueChanged\n" + er.Message);
            }
        }

        private void nudMoveDX_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13) e.SuppressKeyPress = true;
        }

        private void cmbRotate_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void toolStripButton14_Click(object sender, EventArgs e)
        {
            try
            {
                if (SelectedObjectIdx < 0) return;
                if (listModifers.SelectedIndex < 0) return;
                int idx = listModifers.SelectedIndex;
                VisualComponentMethodLock++;
                MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].Add(new MeshModify(0));
                MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].CurrentLength - 1].SetCode(MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[idx].ModifyCode);

                if (MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[idx].intParameters != null)
                    for (int i = 0; i < MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[idx].intParameters.Length; i++)
                        MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].CurrentLength - 1].intParameters[i] =
                                                            MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[idx].intParameters[i];
                if (MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[idx].floatParameters != null)
                    for (int i = 0; i < MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[idx].floatParameters.Length; i++)
                        MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].CurrentLength - 1].floatParameters[i] =
                                                            MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[idx].floatParameters[i];


                MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].RefreshList(listModifers);
                tabModProperties.SelectedIndex = 0;
                listModifers.Focus();
                listModifers.SelectedIndex = listModifers.Items.Count - 1;
                tabModProperties.Visible = true;
                VisualComponentMethodLock--;
                listModifers_SelectedIndexChanged(null, null);
                isEditedProject = true;
                isEditedObject = true;
            }
            catch (Exception er)
            {
                MessageBox.Show("toolStripButton14_Click\n" + er.Message);
                VisualComponentMethodLock--;
            }
        }

        private void toolStripButton15_Click(object sender, EventArgs e)
        {
            try
            {
                if (SelectedObjectIdx < 0) return;
                if (listModifers.SelectedIndex < 1) return;
                VisualComponentMethodLock++;

                int idx = listModifers.SelectedIndex;

                MeshModify tmp = MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex];
                MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex] = MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex - 1];
                MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex - 1] = tmp;

                MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].RefreshList(listModifers);
                listModifers.SelectedIndex = idx - 1;
                VisualComponentMethodLock--;

                isEditedProject = true;
                isEditedObject = true;
            }
            catch (Exception er)
            {
                MessageBox.Show("toolStripButton15_Click\n" + er.Message);
                VisualComponentMethodLock--;
            }
        }

        private void toolStripButton16_Click(object sender, EventArgs e)
        {
            try
            {
                if (SelectedObjectIdx < 0) return;
                if (listModifers.SelectedIndex < 0) return;
                if (listModifers.SelectedIndex >= listModifers.Items.Count - 1) return;
                VisualComponentMethodLock++;

                int idx = listModifers.SelectedIndex;

                MeshModify tmp = MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex];
                MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex] = MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex + 1];
                MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex + 1] = tmp;

                MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].RefreshList(listModifers);
                listModifers.SelectedIndex = idx + 1;
                VisualComponentMethodLock--;

                isEditedProject = true;
                isEditedObject = true;
            }
            catch (Exception er)
            {
                MessageBox.Show("toolStripButton15_Click\n" + er.Message);
                VisualComponentMethodLock--;
            }
        }

        private void rbCenter_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (VisualComponentMethodLock > 0) return;
                if (SelectedObjectIdx < 0) return;
                if (listModifers.SelectedIndex < 0) return;
                VisualComponentMethodLock++;
                MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].intParameters[0] = (int)(((RadioButton)sender).Tag);
                MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].UpdateListBox(listModifers, listModifers.SelectedIndex);
                VisualComponentMethodLock--;
                isEditedProject = true;
                isEditedObject = true;
            }
            catch (Exception er)
            {
                MessageBox.Show("rbCenter_CheckedChanged\n" + er.Message);
                VisualComponentMethodLock--;
            }
        }

        private void сохранитьВФайлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (SelectedObjectIdx < 0)
                {
                    MessageBox.Show("Следует предварительно выбрать модель");
                    return;
                }

                #region Выбираем файл
                if (saveObjectDlg.ShowDialog() != DialogResult.OK) return;
                #endregion

                timerDraw.Enabled = false;

                #region Сохраняем модель в файл
                switch (model[SelectedObjectIdx].ModelType())
                {
                    case ModelTypes.Plane3D:
                        if (((Plane3D)model[SelectedObjectIdx]).SaveModelToFile(saveObjectDlg.FileName) != 0) throw new Exception(ErrorLog.GetLastError());
                        break;
                    case ModelTypes.CellPlane3D:
                        if (((CellPlane3D)model[SelectedObjectIdx]).SaveModelToFile(saveObjectDlg.FileName) != 0) throw new Exception(ErrorLog.GetLastError());
                        break;
                    case ModelTypes.ComplexModel3D:
                        if (((ComplexModel3D)model[SelectedObjectIdx]).SaveModelToFile(saveObjectDlg.FileName) != 0) throw new Exception(ErrorLog.GetLastError());
                        break;
                    case ModelTypes.Cylinder3D:
                        if (((Cylinder3D)model[SelectedObjectIdx]).SaveModelToFile(saveObjectDlg.FileName) != 0) throw new Exception(ErrorLog.GetLastError());
                        break;
                    case ModelTypes.Ellipse3D:
                        if (((Ellipse3D)model[SelectedObjectIdx]).SaveModelToFile(saveObjectDlg.FileName) != 0) throw new Exception(ErrorLog.GetLastError());
                        break;
                    case ModelTypes.Prism3D:
                        if (((Prism3D)model[SelectedObjectIdx]).SaveModelToFile(saveObjectDlg.FileName) != 0) throw new Exception(ErrorLog.GetLastError());
                        break;
                    case ModelTypes.Tor3D:
                        if (((Tor3D)model[SelectedObjectIdx]).SaveModelToFile(saveObjectDlg.FileName) != 0) throw new Exception(ErrorLog.GetLastError());
                        break;
                    case ModelTypes.SurfaceHole3D:
                        if (((SurfaceHole3D)model[SelectedObjectIdx]).SaveModelToFile(saveObjectDlg.FileName) != 0) throw new Exception(ErrorLog.GetLastError());
                        break;
                }
                #endregion

                #region Сохраняем модификаторы
                {
                    #region Определяем шаблон файла
                    string FileName = saveObjectDlg.FileName;
                    int pos = FileName.LastIndexOf(".");
                    if (pos >= 0) FileName = FileName.Substring(0, pos).Trim();
                    if (FileName == "") throw new Exception("Некорректное имя файла");
                    FileName += ".mdy";
                    #endregion
                    MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].SaveToFile(FileName);
                }
                #endregion

                timerDraw.Enabled = true;
            }
            catch (Exception er)
            {
                MessageBox.Show("сохранитьВФайлToolStripMenuItem_Click\n" + er.Message);
                timerDraw.Enabled = true;
            }
        }

        private void сохранитьПроектКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                #region Выбираем файл
                if (saveProjectDlg.ShowDialog() != DialogResult.OK) return;
                #endregion

                #region Сохраняем проект
                SaveProject(saveProjectDlg.FileName);
                isEditedProject = false;
                isEditedObject = false;
                this.Text = "Студия трехмерных моделей - " + saveProjectDlg.FileName;
                #endregion
            }
            catch (Exception er)
            {
                MessageBox.Show("сохранитьПроектКакToolStripMenuItem_Click\n" + er.Message);
            }
            timerDraw.Enabled = true;
        }

        public void SaveProject(string FileName)
        {
            if (projectSaver.Saving) return;
            projectSaver.Saving = true;

            #region Определяем шаблон файла
            {
                int pos = FileName.LastIndexOf(".");
                if (pos >= 0) FileName = FileName.Substring(0, pos).Trim();
            }
            if (FileName == "") throw new Exception("Некорректное имя файла");
            #endregion

            timerDraw.Enabled = false;
            {
                #region Сохранение объектов в временные переменные 
                VolumetricModel3D[] temp = new VolumetricModel3D[UnitCount - CoordLineCount - SelectionLineCount];
                {
                    int n = 0;
                    for (int i = CoordLineCount + SelectionLineCount; i < UnitCount; i++, n++)
                    {
                        #region Записываем полигонный объект в файл на основе главной вершинной модели с сохранением всех начальных параметров
                        switch (model[i].ModelType())
                        {
                            case ModelTypes.ComplexModel3D:
                                temp[n] = new ComplexModel3D();
                                break;
                            case ModelTypes.Plane3D:
                                temp[n] = new Plane3D();
                                break;
                            case ModelTypes.CellPlane3D:
                                temp[n] = new CellPlane3D();
                                break;
                            case ModelTypes.Cylinder3D:
                                temp[n] = new Cylinder3D();
                                break;
                            case ModelTypes.Ellipse3D:
                                temp[n] = new Ellipse3D();
                                break;
                            case ModelTypes.Tor3D:
                                temp[n] = new Tor3D();
                                break;
                            case ModelTypes.Prism3D:
                                temp[n] = new Prism3D();
                                break;
                            case ModelTypes.SurfaceHole3D:
                                temp[n] = new SurfaceHole3D();
                                break;
                        }
                        temp[n].CreateModelFrom(model[i]);
                        #endregion
                    }
                }
                MeshModifications[] tempMesh = new MeshModifications[MaxUnitCount];
                for (int n = 0; n < temp.Length; n++)
                    tempMesh[n] = MeshMod[n] != null ? new MeshModifications(MeshMod[n]) : null;
                #endregion


                timerDraw.Enabled = true;

                Task taskSave = new Task(() =>
                {
                    #region Сохраняем модели в файл
                    for (int i = 0; i < temp.Length; i++)
                    {
                        string sFile = FileName + i.ToString() + ".msh";

                        temp[i].SaveModelToFile(sFile);


                        #region Дописываем модификаторы

                        #region Определяем шаблон файла для i-го объекта               
                        int pos = sFile.LastIndexOf(".");
                        if (pos >= 0) sFile = sFile.Substring(0, pos).Trim();
                        if (sFile == "") throw new Exception("Некорректное имя файла");
                        sFile += ".mdy";
                        #endregion

                        tempMesh[i]?.SaveToFile(sFile);
                        #endregion
                    }
                    #endregion

                    #region Сохраняем информационный файл проекта
                    StreamWriter file = new StreamWriter(FileName + ".pjm", false, System.Text.Encoding.Default);
                    try
                    {
                        CultureInfo culture = CultureInfo.InvariantCulture;
                        file.WriteLine("* Файл проекта 3D-моделей");
                        file.WriteLine("* Время создания файла: " + DateTime.Now.ToString());
                        file.WriteLine("Model_Count=" + (UnitCount - CoordLineCount - SelectionLineCount).ToString());
                        file.WriteLine("ModifyFilesFormat=Separated");
                    }
                    finally
                    {
                        file.Close();
                        file.Dispose();
                        projectSaver.Saving = false;
                    }
                    #endregion
                });
                taskSave.Start();
            }
        }

        public void OpenProject(string FileName, bool isRecovery = false, bool fAdd = false)
        {
            try
            {
                if (!isRecovery) this.Text = "Студия трехмерных моделей - " + FileName;

                VisualComponentMethodLock++;

                #region Определяем шаблон файла
                {
                    int pos = FileName.LastIndexOf(".");
                    if (pos >= 0) FileName = FileName.Substring(0, pos).Trim();
                }
                if (FileName == "") throw new Exception("Некорректное имя файла");
                #endregion

                timerDraw.Enabled = false;

                #region Считываем информационный файл проекта
                CultureInfo culture = CultureInfo.InvariantCulture;
                StreamReader file = new StreamReader(FileName + ".pjm", System.Text.Encoding.Default);
                int model_count = 0;
                try
                {
                    string sLine = "";

                    while (!file.EndOfStream)
                    {
                        sLine = file.ReadLine().Trim();
                        if (sLine == "") continue;
                        if (sLine.Substring(0, 1) == "*") continue;

                        int semIdx = sLine.IndexOf("=");
                        if (semIdx > 0)
                        {
                            string PropertyName = sLine.Substring(0, semIdx).Trim();
                            string PropertyValue = sLine.Substring(semIdx + 1, sLine.Length - semIdx - 1).Trim();
                            if ((PropertyName == "") | (PropertyValue == "")) throw new Exception("Неизвестный формата файла: некорретный формат свойства: " + sLine);

                            switch (PropertyName)
                            {
                                case "Model_Count":
                                    model_count = int.Parse(PropertyValue);
                                    break;
                            }
                        }
                    }
                }
                finally
                {
                    file.Close();
                    file.Dispose();
                }
                #endregion

                #region Читаем модели из файлов
                for (int i = 0; i < model_count; i++)
                {
                    string sFile = FileName + i.ToString() + ".msh";

                    AddObject(sFile);

                    timerDraw.Enabled = true;
                }
                #endregion

                if (!isRecovery)
                {
                    if (!fAdd)
                    {
                        CurrentAutoSaveCount = 0;
                        CurrentAutoSaveIndex = 0;
                        RedoAutoSaveIndex = 0;
                        toolStripButton3.Enabled = false;
                        toolStripButton10.Enabled = false;
                    }
                    isEditedProject = false;
                    isEditedObject = false;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show("OpenProject\n" + er.Message);
            }
            VisualComponentMethodLock--;
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (NewProject() == DialogResult.Cancel) return;
                if (openProjectDlg.ShowDialog() != DialogResult.OK) return;
                saveProjectDlg.FileName = openProjectDlg.FileName;
                OpenProject(openProjectDlg.FileName);
                isEditedObject = true;
            }
            catch (Exception er)
            {
                MessageBox.Show("открытьToolStripMenuItem_Click\n" + er.Message);
            }
        }

        private void координатныеОсиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (координатныеОсиToolStripMenuItem.Checked)
            {
                for (int i = 0; i < ModelAxis.Length; i++)
                    ModelAxis[i].SetColor(Color.FromArgb(100, ModelAxis[i].Polygon[0].color[0]));
            }
            else
            {
                for (int i = 0; i < ModelAxis.Length; i++)
                    ModelAxis[i].SetColor(Color.FromArgb(0, ModelAxis[i].Polygon[0].color[0]));
            }
        }

        private void добавитьПроектToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (openProjectDlg.ShowDialog() != DialogResult.OK) return;
                string txt = this.Text;
                OpenProject(openProjectDlg.FileName, false, true);
                this.Text = txt;
                isEditedObject = true;
                isEditedProject = true;
            }
            catch (Exception er)
            {
                MessageBox.Show("добавитьПроектToolStripMenuItem_Click\n" + er.Message);
            }
        }

        private void nudSettingN1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13) e.SuppressKeyPress = true;
        }

        public void SetGeometryParameters()
        {
            try
            {
                if (VisualComponentMethodLock > 0) return;

                #region Опорные точки
                {
                    switch (model[SelectedObjectIdx].ModelType())
                    {
                        case ModelTypes.Plane3D:
                            if ((int)nudSettingN1.Value < 4) nudSettingN1.Value = 4;
                            ((Plane3D)model[SelectedObjectIdx]).N = (int)nudSettingN1.Value;
                            break;
                        case ModelTypes.CellPlane3D:
                            if ((int)nudSettingN1.Value < 2) nudSettingN1.Value = 2;
                            ((CellPlane3D)model[SelectedObjectIdx])._Nx = (int)nudSettingN1.Value;
                            break;
                        case ModelTypes.Cylinder3D:
                            if ((int)nudSettingN1.Value < 3) nudSettingN1.Value = 3;
                            ((Cylinder3D)model[SelectedObjectIdx]).N = (int)nudSettingN1.Value;
                            break;
                        case ModelTypes.Ellipse3D:
                            if ((int)nudSettingN1.Value < 3) nudSettingN1.Value = 3;
                            ((Ellipse3D)model[SelectedObjectIdx]).Nx = (int)nudSettingN1.Value;
                            break;
                        case ModelTypes.Tor3D:
                            if ((int)nudSettingN1.Value < 2) nudSettingN1.Value = 2;
                            ((Tor3D)model[SelectedObjectIdx]).Nx = (int)nudSettingN1.Value;
                            break;
                        case ModelTypes.Prism3D:
                            if ((int)nudSettingN1.Value < 1) nudSettingN1.Value = 1;
                            ((Prism3D)model[SelectedObjectIdx]).nWidth = (int)nudSettingN1.Value;
                            break;
                        case ModelTypes.SurfaceHole3D:
                            if ((int)nudSettingN1.Value < 2) nudSettingN1.Value = 2;
                            ((SurfaceHole3D)model[SelectedObjectIdx]).Nx = (int)nudSettingN1.Value;
                            break;
                    }

                    switch (model[SelectedObjectIdx].ModelType())
                    {
                        case ModelTypes.CellPlane3D:
                            if ((int)nudSettingN2.Value < 2) nudSettingN2.Value = 2;
                            ((CellPlane3D)model[SelectedObjectIdx])._Nz = (int)nudSettingN2.Value;
                            break;
                        case ModelTypes.Cylinder3D:
                            if ((int)nudSettingN2.Value < 1) nudSettingN2.Value = 1;
                            ((Cylinder3D)model[SelectedObjectIdx]).sectionN = (int)nudSettingN2.Value;
                            break;
                        case ModelTypes.Ellipse3D:
                            if ((int)nudSettingN2.Value < 3) nudSettingN2.Value = 3;
                            ((Ellipse3D)model[SelectedObjectIdx]).Nz = (int)nudSettingN2.Value;
                            break;
                        case ModelTypes.Tor3D:
                            if ((int)nudSettingN2.Value < 2) nudSettingN2.Value = 2;
                            ((Tor3D)model[SelectedObjectIdx]).Nz = (int)nudSettingN2.Value;
                            break;
                        case ModelTypes.Prism3D:
                            if ((int)nudSettingN2.Value < 1) nudSettingN2.Value = 1;
                            ((Prism3D)model[SelectedObjectIdx]).nHeight = (int)nudSettingN2.Value;
                            break;
                        case ModelTypes.SurfaceHole3D:
                            if ((int)nudSettingN2.Value < 2) nudSettingN2.Value = 2;
                            ((SurfaceHole3D)model[SelectedObjectIdx]).Nz = (int)nudSettingN2.Value;
                            break;
                    }

                    switch (model[SelectedObjectIdx].ModelType())
                    {
                        case ModelTypes.Prism3D:
                            if ((int)nudSettingN3.Value < 1) nudSettingN3.Value = 1;
                            ((Prism3D)model[SelectedObjectIdx]).nDepth = (int)nudSettingN3.Value;
                            break;
                    }
                }
                #endregion

                #region Углы развертки
                switch (model[SelectedObjectIdx].ModelType())
                {
                    case ModelTypes.Plane3D:
                        ((Plane3D)model[SelectedObjectIdx]).AngleStart = (float)nudAngle1Start.Value * Engine3D.RadianDegrees;
                        ((Plane3D)model[SelectedObjectIdx]).AngleFinish = (float)nudAngle1Finish.Value * Engine3D.RadianDegrees;
                        break;
                    case ModelTypes.Cylinder3D:
                        ((Cylinder3D)model[SelectedObjectIdx]).AngleStart = (float)nudAngle1Start.Value * Engine3D.RadianDegrees;
                        ((Cylinder3D)model[SelectedObjectIdx]).AngleFinish = (float)nudAngle1Finish.Value * Engine3D.RadianDegrees;
                        break;
                    case ModelTypes.Ellipse3D:
                        ((Ellipse3D)model[SelectedObjectIdx]).StartAngleXY = (float)nudAngle1Start.Value * Engine3D.RadianDegrees;
                        ((Ellipse3D)model[SelectedObjectIdx]).FinishAngleXY = (float)nudAngle1Finish.Value * Engine3D.RadianDegrees;
                        ((Ellipse3D)model[SelectedObjectIdx]).StartAngleYZ = (float)nudAngle2Start.Value * Engine3D.RadianDegrees;
                        ((Ellipse3D)model[SelectedObjectIdx]).FinishAngleYZ = (float)nudAngle2Finish.Value * Engine3D.RadianDegrees;
                        break;
                    case ModelTypes.Tor3D:
                        ((Tor3D)model[SelectedObjectIdx]).StartAngleXY = (float)nudAngle1Start.Value * Engine3D.RadianDegrees;
                        ((Tor3D)model[SelectedObjectIdx]).FinishAngleXY = (float)nudAngle1Finish.Value * Engine3D.RadianDegrees;
                        ((Tor3D)model[SelectedObjectIdx]).StartAngleYZ = (float)nudAngle2Start.Value * Engine3D.RadianDegrees;
                        ((Tor3D)model[SelectedObjectIdx]).FinishAngleYZ = (float)nudAngle2Finish.Value * Engine3D.RadianDegrees;
                        break;
                }
                #endregion

                #region  Панели
                switch (model[SelectedObjectIdx].ModelType())
                {
                    case ModelTypes.Cylinder3D:
                        ((Cylinder3D)model[SelectedObjectIdx]).BottomVisible = chkBottom.Checked;
                        ((Cylinder3D)model[SelectedObjectIdx]).TopVisible = chkTop.Checked;
                        break;
                    case ModelTypes.Prism3D:
                        if ((!chkBottom.Checked) & (!chkTop.Checked) & (!chkLeft.Checked) & (!chkRight.Checked) & (!chkFront.Checked) & (!chkRear.Checked))
                        {
                            MessageBox.Show("Хотя бы одна панель должна быть видимой");
                            chkTop.Checked = true;
                        }
                        ((Prism3D)model[SelectedObjectIdx]).BottomVisible = chkBottom.Checked;
                        ((Prism3D)model[SelectedObjectIdx]).TopVisible = chkTop.Checked;
                        ((Prism3D)model[SelectedObjectIdx]).LeftVisible = chkLeft.Checked;
                        ((Prism3D)model[SelectedObjectIdx]).RightVisible = chkRight.Checked;
                        ((Prism3D)model[SelectedObjectIdx]).FrontVisible = chkFront.Checked;
                        ((Prism3D)model[SelectedObjectIdx]).RearVisible = chkRear.Checked;
                        break;
                }
                #endregion

                #region Начальные размеры
                switch (model[SelectedObjectIdx].ModelType())
                {
                    case ModelTypes.Plane3D:
                        ((Plane3D)model[SelectedObjectIdx]).RxPlus = (float)NudR1Xplus.Value;
                        ((Plane3D)model[SelectedObjectIdx]).RxMinus = (float)NudR1Xminus.Value;
                        ((Plane3D)model[SelectedObjectIdx]).RyPlus = (float)NudR1Yplus.Value;
                        ((Plane3D)model[SelectedObjectIdx]).RyMinus = (float)NudR1Yminus.Value;
                        break;
                    case ModelTypes.CellPlane3D:
                        ((CellPlane3D)model[SelectedObjectIdx])._WidthX = (int)NudR1Xplus.Value;
                        ((CellPlane3D)model[SelectedObjectIdx])._WidthZ = (int)NudR1Zplus.Value;
                        break;
                    case ModelTypes.Cylinder3D:
                        ((Cylinder3D)model[SelectedObjectIdx]).RBottomXpositive = (float)NudR1Xplus.Value;
                        ((Cylinder3D)model[SelectedObjectIdx]).RBottomXnegative = (float)NudR1Xminus.Value;
                        ((Cylinder3D)model[SelectedObjectIdx]).RBottomZpositive = (float)NudR1Zplus.Value;
                        ((Cylinder3D)model[SelectedObjectIdx]).RBottomZnegative = (float)NudR1Zminus.Value;
                        ((Cylinder3D)model[SelectedObjectIdx]).RTopXpositive = (float)NudR2Xplus.Value;
                        ((Cylinder3D)model[SelectedObjectIdx]).RTopXnegative = (float)NudR2Xminus.Value;
                        ((Cylinder3D)model[SelectedObjectIdx]).RTopZpositive = (float)NudR2Zplus.Value;
                        ((Cylinder3D)model[SelectedObjectIdx]).RTopZnegative = (float)NudR2Zminus.Value;
                        ((Cylinder3D)model[SelectedObjectIdx]).Height = (float)NudR1Yplus.Value;
                        break;
                    case ModelTypes.Prism3D:
                        ((Prism3D)model[SelectedObjectIdx]).Width = (int)NudR1Xplus.Value;
                        ((Prism3D)model[SelectedObjectIdx]).Height = (int)NudR1Yplus.Value;
                        ((Prism3D)model[SelectedObjectIdx]).Depth = (int)NudR1Zplus.Value;
                        break;
                    case ModelTypes.Ellipse3D:
                        ((Ellipse3D)model[SelectedObjectIdx]).RxPositive = (float)NudR1Xplus.Value;
                        ((Ellipse3D)model[SelectedObjectIdx]).RxNegative = (float)NudR1Xminus.Value;
                        ((Ellipse3D)model[SelectedObjectIdx]).RyPositive = (float)NudR1Yplus.Value;
                        ((Ellipse3D)model[SelectedObjectIdx]).RyNegative = (float)NudR1Yminus.Value;
                        ((Ellipse3D)model[SelectedObjectIdx]).RzPositive = (float)NudR1Zplus.Value;
                        ((Ellipse3D)model[SelectedObjectIdx]).RzNegative = (float)NudR1Zminus.Value;
                        break;
                    case ModelTypes.Tor3D:
                        ((Tor3D)model[SelectedObjectIdx]).RxPositive = (float)NudR1Xplus.Value;
                        ((Tor3D)model[SelectedObjectIdx]).RxNegative = (float)NudR1Xminus.Value;
                        ((Tor3D)model[SelectedObjectIdx]).RyPositive = (float)NudR1Yplus.Value;
                        ((Tor3D)model[SelectedObjectIdx]).RyNegative = (float)NudR1Yminus.Value;
                        ((Tor3D)model[SelectedObjectIdx]).RADxPositive = (float)NudR2Xplus.Value;
                        ((Tor3D)model[SelectedObjectIdx]).RADxNegative = (float)NudR2Xminus.Value;
                        ((Tor3D)model[SelectedObjectIdx]).RADyPositive = (float)NudR2Yplus.Value;
                        ((Tor3D)model[SelectedObjectIdx]).RADyNegative = (float)NudR2Yminus.Value;
                        ((Tor3D)model[SelectedObjectIdx]).RADzPositive = (float)NudR2Zplus.Value;
                        ((Tor3D)model[SelectedObjectIdx]).RADzNegative = (float)NudR2Zminus.Value;
                        break;
                    case ModelTypes.SurfaceHole3D:
                        ((SurfaceHole3D)model[SelectedObjectIdx]).RXpositive = (float)NudR1Xplus.Value;
                        ((SurfaceHole3D)model[SelectedObjectIdx]).RXnegative = (float)NudR1Xminus.Value;
                        ((SurfaceHole3D)model[SelectedObjectIdx]).RZpositive = (float)NudR1Zplus.Value;
                        ((SurfaceHole3D)model[SelectedObjectIdx]).RZnegative = (float)NudR1Zminus.Value;
                        break;
                }
                #endregion

                #region Пересчет модели и обновление меток выбора
                model[SelectedObjectIdx].RebuildModel(true);
                if (modelController.CreateActivePolygonBuffer() != 0) throw new Exception(ErrorLog.GetLastError());
                #endregion

                isEditedProject = true;
                isEditedObject = true;
            }
            catch (Exception er)
            {
                MessageBox.Show("SetGeometryParameters\n" + er.Message);
            }
        }

        private void trackMatte1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                lblMatteVal1.Text = "Матовость внешняя: " + trackMatte1.Value.ToString() + "%";
                if (VisualComponentMethodLock > 0) return;
                if (lblMatte1.Visible) return;
                model[SelectedObjectIdx].SetMatte(((float)trackMatte1.Value) / 100, 0, -1, PolygonSides.FrontSide);
                isEditedProject = true;
                isEditedObject = true;
            }
            catch (Exception er)
            {
                MessageBox.Show("trackMatte1_ValueChanged\n" + er.Message);
            }
        }

        private void trackMatte2_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                lblMatteVal2.Text = "Матовость внутренняя: " + trackMatte2.Value.ToString() + "%";
                if (VisualComponentMethodLock > 0) return;
                if (lblMatte2.Visible) return;
                model[SelectedObjectIdx].SetMatte(((float)trackMatte2.Value) / 100, 0, -1, PolygonSides.RearSide);
                isEditedProject = true;
                isEditedObject = true;
            }
            catch (Exception er)
            {
                MessageBox.Show("trackMatte2_ValueChanged\n" + er.Message);
            }
        }

        private void btnGradientExt_Click(object sender, EventArgs e)
        {
            try
            {
                if (SelectedObjectIdx < 0) return;
                timerDraw.Enabled = false;
                frmColorPaint frm = new frmColorPaint();
                if (model[SelectedObjectIdx].ResetActivePolygonIndexes() != 0) throw new Exception(ErrorLog.GetLastError());
                if (model[SelectedObjectIdx].ResetCameraModel() != 0) throw new Exception(ErrorLog.GetLastError());
                ModifyModel(model[SelectedObjectIdx], MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount]);
                if (model[SelectedObjectIdx].CalculatePolygonCenters() != 0) throw new Exception(ErrorLog.GetLastError());
                if (frm.Run(ref model[SelectedObjectIdx], PolygonSides.FrontSide) == DialogResult.OK)
                {
                    MouseSelectedObjectIdx = SelectedObjectIdx;
                    MouseSelectObject();
                    isEditedProject = true;
                    isEditedObject = true;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show("btnGradientExt_Click\n" + er.Message);
            }
            timerDraw.Enabled = true;
        }

        private void btnPaintInt_Click(object sender, EventArgs e)
        {
            try
            {
                if (SelectedObjectIdx < 0) return;
                timerDraw.Enabled = false;
                frmColorPaint frm = new frmColorPaint();
                if (model[SelectedObjectIdx].ResetActivePolygonIndexes() != 0) throw new Exception(ErrorLog.GetLastError());
                if (model[SelectedObjectIdx].ResetCameraModel() != 0) throw new Exception(ErrorLog.GetLastError());
                ModifyModel(model[SelectedObjectIdx], MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount]);
                if (model[SelectedObjectIdx].CalculatePolygonCenters() != 0) throw new Exception(ErrorLog.GetLastError());
                if (frm.Run(ref model[SelectedObjectIdx], PolygonSides.RearSide) != DialogResult.OK)
                {
                    MouseSelectedObjectIdx = SelectedObjectIdx;
                    MouseSelectObject();
                    isEditedProject = true;
                    isEditedObject = true;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show("btnPaintInt_Click\n" + er.Message);
            }
            timerDraw.Enabled = true;
        }

        private void timerAutoSaveChanges_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!isEditedObject) return;

                timerDraw.Enabled = false;
                timerAutoSaveChanges.Enabled = false;

                SaveProject(TempProjectName + "-" + CurrentAutoSaveIndex.ToString() + ".tmp.temp");

                CurrentAutoSaveIndex++;
                CurrentAutoSaveCount++;

                if (CurrentAutoSaveIndex >= MaxAutoSaveBufferSize) CurrentAutoSaveIndex = 0;
                if (CurrentAutoSaveCount > MaxAutoSaveBufferSize) CurrentAutoSaveCount = MaxAutoSaveBufferSize;

                RedoAutoSaveIndex = CurrentAutoSaveIndex;
                toolStripButton10.Enabled = false;
                toolStripButton3.Enabled = CurrentAutoSaveCount > 1;


                isEditedObject = false;
                timerAutoSaveChanges.Enabled = true;
            }
            catch (Exception er)
            {
                MessageBox.Show("Ошибка при автосохранении.\n" + er.Message);
            }
            timerDraw.Enabled = true;
        }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            try
            {
                if (CurrentAutoSaveIndex == RedoAutoSaveIndex) return;

                timerDraw.Enabled = false;
                timerAutoSaveChanges.Enabled = false;

                CurrentAutoSaveIndex++;
                CurrentAutoSaveCount++;

                if (CurrentAutoSaveIndex >= MaxAutoSaveBufferSize) CurrentAutoSaveIndex = 0;
                if (CurrentAutoSaveCount > MaxAutoSaveBufferSize) CurrentAutoSaveCount = MaxAutoSaveBufferSize;

                int idx = CurrentAutoSaveIndex - 1;
                if (idx < 0) idx = MaxAutoSaveBufferSize - 1;

                NewProject(true);
                OpenProject(TempProjectName + "-" + idx.ToString() + ".tmp.temp", true);

                toolStripButton10.Enabled = CurrentAutoSaveIndex != RedoAutoSaveIndex;
                toolStripButton3.Enabled = CurrentAutoSaveCount > 1;

                isEditedObject = false;
                timerAutoSaveChanges.Enabled = true;
            }
            catch (Exception er)
            {
                MessageBox.Show("Ошибка возврата изменения.\n" + er.Message);
            }
            timerAutoSaveChanges.Enabled = true;
            timerDraw.Enabled = true;
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentAutoSaveIndex--;
                CurrentAutoSaveCount--;
                if (CurrentAutoSaveCount <= 0)
                {
                    CurrentAutoSaveIndex++;
                    CurrentAutoSaveCount++;
                    return;
                }

                timerDraw.Enabled = false;
                timerAutoSaveChanges.Enabled = false;

                if (CurrentAutoSaveIndex < 0) CurrentAutoSaveIndex = MaxAutoSaveBufferSize - 1;

                int idx = CurrentAutoSaveIndex - 1;
                if (idx < 0) idx = MaxAutoSaveBufferSize - 1;

                NewProject(true);
                OpenProject(TempProjectName + "-" + idx.ToString() + ".tmp.temp", true);

                toolStripButton10.Enabled = CurrentAutoSaveIndex != RedoAutoSaveIndex;
                toolStripButton3.Enabled = CurrentAutoSaveCount > 1;
                isEditedObject = false;
                timerAutoSaveChanges.Enabled = true;
            }
            catch (Exception er)
            {
                MessageBox.Show("Ошибка отмены изменения.\n" + er.Message);
            }
            timerAutoSaveChanges.Enabled = true;
            timerDraw.Enabled = true;
        }

        private void pictMain_Click(object sender, EventArgs e)
        {

        }

        private void управлениеКурсоромToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void nudTwistAngleTop_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (VisualComponentMethodLock > 0) return;
                if (SelectedObjectIdx < 0) return;
                if (listModifers.SelectedIndex < 0) return;
                VisualComponentMethodLock++;
                MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].floatParameters[1] = (float)nudTwistAngleTop.Value * Engine3D.RadianDegrees;
                MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].floatParameters[0] = (float)nudTwistAngleBottom.Value * Engine3D.RadianDegrees;
                MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].floatParameters[3] = (float)nudTwistLevelTop.Value;
                MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].floatParameters[2] = (float)nudTwistLevelBottom.Value;
                MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].intParameters[0] = checkTwistDown.Checked ? 1 : 0;
                MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].intParameters[1] = checkTwistTop.Checked ? 1 : 0;
                MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].UpdateListBox(listModifers, listModifers.SelectedIndex);
                VisualComponentMethodLock--;
                isEditedProject = true;
                isEditedObject = true;
            }
            catch (Exception er)
            {
                MessageBox.Show("nudTwistAngleTop_ValueChanged\n" + er.Message);
                VisualComponentMethodLock--;
            }
        }

        private void nudTwistAngleTop_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13) e.SuppressKeyPress = true;
        }

        private void TabsheetProperties_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void nudBendAngle_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (VisualComponentMethodLock > 0) return;
                if (SelectedObjectIdx < 0) return;
                if (listModifers.SelectedIndex < 0) return;
                VisualComponentMethodLock++;
                MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].floatParameters[0] = (float)nudBendAngle.Value * Engine3D.RadianDegrees;
                MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].floatParameters[1] = (float)nudBendBottom.Value;
                MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].floatParameters[2] = (float)nudBendTop.Value;
                MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].UpdateListBox(listModifers, listModifers.SelectedIndex);
                VisualComponentMethodLock--;
                isEditedProject = true;
                isEditedObject = true;
            }
            catch (Exception er)
            {
                MessageBox.Show("nudBendAngle_ValueChanged\n" + er.Message);
                VisualComponentMethodLock--;
            }
        }

        private void nudBendAngle_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13) e.SuppressKeyPress = true;
        }

        private void nudULFx_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13) e.SuppressKeyPress = true;
        }

        private void nudULFx_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (VisualComponentMethodLock > 0) return;
                if (SelectedObjectIdx < 0) return;
                if (listModifers.SelectedIndex < 0) return;
                VisualComponentMethodLock++;

                MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].floatParameters[0] = (float)nudCollapseXmin.Value;
                MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].floatParameters[1] = (float)nudCollapseXmax.Value;
                MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].floatParameters[2] = (float)nudCollapseYmin.Value;
                MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].floatParameters[3] = (float)nudCollapseYmax.Value;
                MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].floatParameters[4] = (float)nudCollapseZmin.Value;
                MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].floatParameters[5] = (float)nudCollapseZmax.Value;
                MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].floatParameters[6] = (float)nudCollapseDY1.Value;
                MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].floatParameters[7] = (float)nudCollapseDY2.Value;

                MeshMod[SelectedObjectIdx - CoordLineCount - SelectionLineCount].MeshInfo[listModifers.SelectedIndex].UpdateListBox(listModifers, listModifers.SelectedIndex);
                VisualComponentMethodLock--;
                isEditedProject = true;
                isEditedObject = true;
            }
            catch (Exception er)
            {
                MessageBox.Show("nudULFx_ValueChanged\n" + er.Message);
                VisualComponentMethodLock--;
            }
        }

        private void NudGrdSphX_ValueChanged(object sender, EventArgs e)
        {
        }

        private void panelGrdSphColorIn_Click(object sender, EventArgs e)
        {

        }

        private void panelGrdSphColorOut_Click(object sender, EventArgs e)
        {

        }

        private void NudGrdSphX_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13) e.SuppressKeyPress = true;
        }

        private void toolStripButton18_Click(object sender, EventArgs e)
        {
            timerDraw.Enabled = false;
            AddObject(ModelTypes.SurfaceHole3D);
            timerDraw.Enabled = true;
        }

        private void txtExtColorCode_TextChanged(object sender, EventArgs e)
        {
        }

        private void nudColorExtCode_ValueChanged(object sender, EventArgs e)
        {
            if (VisualComponentMethodLock > 0) return;
            try
            {
                panelColor1.BackColor = Color.FromArgb((int)nudColorExtCode.Value);
                model[SelectedObjectIdx].SetColor(panelColor1.BackColor, 0, -1, PolygonSides.FrontSide);
            }
            catch
            { }
        }

        private void nudColorIntCode_ValueChanged(object sender, EventArgs e)
        {
            if (VisualComponentMethodLock > 0) return;
            try
            {
                panelColor2.BackColor = Color.FromArgb((int)nudColorIntCode.Value);
                model[SelectedObjectIdx].SetColor(panelColor2.BackColor, 0, -1, PolygonSides.RearSide);
            }
            catch
            { }
        }

        private void сохранитьВРастровыйФайлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                frmSaveImage frm = new frmSaveImage();
                frm.ShowDialog();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }

        private void рамкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmFrameSetting frm = new frmFrameSetting();
            frm.ShowDialog();
        }

        private void инструкцияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(ApplicationFolder + "\\info.pdf");
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }

        private void управлениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmCursorSettings().ShowDialog();
        }

        private void центрироватьПроектОтносительноНачалаКоординатToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Point3D modelCenter = new Point3D();
                Point3D minPos = new Point3D();
                Point3D maxPos = new Point3D();

                for (int i = CoordLineCount + SelectionLineCount; i < UnitCount; i++)
                {
                    model[i].ResetCameraModel();
                    ModifyModel(model[i], MeshMod[i - CoordLineCount - SelectionLineCount]);
                    if (model[i].CenterVertexValue(ref modelCenter, 1) != 0) throw new Exception(Graph3DLibrary.ErrorLog.GetLastError());

                    if (i == CoordLineCount + SelectionLineCount)
                    {
                        minPos.CopyFrom(modelCenter);
                        maxPos.CopyFrom(modelCenter);
                    }
                    else
                    {
                        minPos.X = Math.Min(minPos.X, modelCenter.X);
                        minPos.Y = Math.Min(minPos.Y, modelCenter.Y);
                        minPos.Z = Math.Min(minPos.Z, modelCenter.Z);

                        maxPos.X = Math.Max(maxPos.X, modelCenter.X);
                        maxPos.Y = Math.Max(maxPos.Y, modelCenter.Y);
                        maxPos.Z = Math.Max(maxPos.Z, modelCenter.Z);
                    }
                }

                modelCenter.CopyFrom((minPos.X + maxPos.X) / 2, (minPos.Y + maxPos.Y) / 2, (minPos.Z + maxPos.Z) / 2);

                for (int i = CoordLineCount + SelectionLineCount; i < UnitCount; i++)
                {
                    int meshModIdx = i - CoordLineCount - SelectionLineCount;
                    MeshMod[meshModIdx].Add(new MeshModify(0));
                    MeshMod[meshModIdx].MeshInfo[MeshMod[meshModIdx].CurrentLength - 1].floatParameters[0] = -modelCenter.X;
                    MeshMod[meshModIdx].MeshInfo[MeshMod[meshModIdx].CurrentLength - 1].floatParameters[1] = -modelCenter.Y;
                    MeshMod[meshModIdx].MeshInfo[MeshMod[meshModIdx].CurrentLength - 1].floatParameters[2] = -modelCenter.Z;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }

        private void объединениеВершинВыбраннойМоделиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ComplexModel3D tmpBack = new ComplexModel3D(model[SelectedObjectIdx]);
            try
            {
                if (SelectedObjectIdx < 0) throw new Exception("Следует предварительно выбрать модель");
                if (model[SelectedObjectIdx].ModelType() != ModelTypes.ComplexModel3D) throw new Exception("Данную операцию можно проводить только с комплексным типом модели.");

                timerDraw.Enabled = false;
                lblWait.Visible = true;
                Application.DoEvents();
                try
                {
                    int beforeVertex = model[SelectedObjectIdx].MainVertex3D.Length;
                    int beforePoly = model[SelectedObjectIdx].Polygon.Length;
                    switch (((ComplexModel3D)model[SelectedObjectIdx]).MergeNearVertex(new frmMergeNearVertex().Run()))
                    {
                        case 0: break;
                        case -1: throw new Exception(Graph3DLibrary.ErrorLog.GetLastError());
                        case -2:
                            model[SelectedObjectIdx] = new ComplexModel3D(tmpBack);
                            InitModelController();
                            timerDraw.Enabled = true;
                            throw new Exception(Graph3DLibrary.ErrorLog.GetLastError());
                        default: throw new Exception(Graph3DLibrary.ErrorLog.GetLastError());
                    }
                    model[SelectedObjectIdx].ResetCameraModel();
                    model[SelectedObjectIdx].ResetActivePolygonIndexes();
                    model[SelectedObjectIdx].ResetLighting();
                    int afterVertex = model[SelectedObjectIdx].MainVertex3D.Length;
                    int afterPoly = model[SelectedObjectIdx].Polygon.Length;
                    MessageBox.Show("Удалено " + (beforeVertex - afterVertex).ToString() + " (" + Math.Round(100f * (beforeVertex - afterVertex) / beforeVertex, 2).ToString("0.00") + "%) вершин и " + (beforePoly - afterPoly).ToString() + " (" + Math.Round(100f * (beforePoly - afterPoly) / beforePoly, 2).ToString("0.00") + "%) полигонов.");
                    isEditedProject = true;
                    isEditedObject = true;
                }
                finally
                {
                    lblWait.Visible = false;
                }
                timerDraw.Enabled = true;
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }

        private void TechInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }

    public class MeshModifications
    {
        public MeshModify[] MeshInfo { get; private set; }
        public int CurrentLength { get; private set; }

        public MeshModifications(int max_lenght)
        {
            if (max_lenght <= 0) throw new Exception("Некорректное значение максимальной длинны");
            MeshInfo = new MeshModify[max_lenght];
            CurrentLength = 0;
        }

        public MeshModifications(MeshModifications template)
        {
            MeshInfo = new MeshModify[template.MeshInfo.Length];
            for (int i = 0; i < MeshInfo.Length; i++)
                MeshInfo[i] = template?.MeshInfo[i] != null ? new MeshModify(template?.MeshInfo[i]) : null;
            CurrentLength = template.CurrentLength;
        }

        public void Add(MeshModify info, int index = -1)
        {
            if (CurrentLength >= MeshInfo.Length) throw new Exception("MeshModifications.Add: недостаточный размер буфера модификаций");
            if ((index > CurrentLength) | (index >= MeshInfo.Length)) throw new Exception("MeshModifications.Add: задан слишком большой индекс для вставки.");
            if (index < 0) index = CurrentLength;
            MeshInfo[index] = info;
            CurrentLength++;
        }

        public void AddCopy(MeshModify info, int index = -1)
        {
            if (CurrentLength >= MeshInfo.Length) throw new Exception("MeshModifications.Add: недостаточный размер буфера модификаций");
            if ((index > CurrentLength) | (index >= MeshInfo.Length)) throw new Exception("MeshModifications.Add: задан слишком большой индекс для вставки.");
            if (index < 0) index = CurrentLength;
            MeshInfo[index] = new MeshModify(info);
            CurrentLength++;
        }


        public void Remove(int index)
        {
            if ((index >= CurrentLength) | (index < 0)) throw new Exception("MeshModifications.Remove: задан некорректный индекс.");

            CurrentLength--;
            for (int i = index; i < CurrentLength; i++) MeshInfo[i] = MeshInfo[i + 1];
            MeshInfo[CurrentLength] = null;
        }

        public void Clear()
        {
            for (int i = 0; i < CurrentLength; i++) MeshInfo[i] = null;
            CurrentLength = 0;
        }

        public void RefreshList(ListBox lv)
        {
            lv.Items.Clear();
            for (int i = 0; i < CurrentLength; i++) MeshInfo[i].WriteToListBox(lv);
        }

        public void Swap(int idx1, int idx2)
        {
            if ((idx1 >= CurrentLength) | (idx1 < 0)) throw new Exception("MeshModifications.Swap: задан некорректный индекс.");
            if ((idx2 >= CurrentLength) | (idx2 < 0)) throw new Exception("MeshModifications.Swap: задан некорректный индекс.");
            if (idx1 == idx2) return;
            MeshModify tmp = MeshInfo[idx1];
            MeshInfo[idx1] = MeshInfo[idx2];
            MeshInfo[idx2] = tmp;
        }

        public void SaveToFile(string FileName, bool AppendFile = false)
        {
            StreamWriter file = new StreamWriter(FileName, AppendFile, System.Text.Encoding.Default);

            try
            {
                CultureInfo culture = CultureInfo.InvariantCulture;

                file.WriteLine("#Modify#");
                file.WriteLine("* Модификации: количество модификаций, [код модификации, количество int-параметров, список int-параметров, количество float-параметров, список float-параметров]");

                file.WriteLine(CurrentLength);
                for (int i = 0; i < CurrentLength; i++)
                {
                    file.WriteLine(MeshInfo[i].ModifyCode.ToString());
                    if (MeshInfo[i].intParameters != null)
                    {
                        file.WriteLine(MeshInfo[i].intParameters.Length.ToString());
                        for (int j = 0; j < MeshInfo[i].intParameters.Length; j++) file.WriteLine(MeshInfo[i].intParameters[j].ToString());
                    }
                    else file.WriteLine("0");

                    if (MeshInfo[i].floatParameters != null)
                    {
                        file.WriteLine(MeshInfo[i].floatParameters.Length.ToString());
                        for (int j = 0; j < MeshInfo[i].floatParameters.Length; j++) file.WriteLine(MeshInfo[i].floatParameters[j].ToString("0.0000", culture));
                    }
                    else file.WriteLine("0");
                }
            }
            finally
            {
                file.Close();
                file.Dispose();
            }
        }

        public void LoadFromFile(string FileName, bool fAdd = false)
        {
            if (!File.Exists(FileName)) return;

            StreamReader file = new StreamReader(FileName, System.Text.Encoding.Default);

            try
            {
                CultureInfo culture = CultureInfo.InvariantCulture;
                string sLine = "";
                int currentMode = -1;

                while (!file.EndOfStream)
                {
                    sLine = file.ReadLine().Trim();
                    if (sLine == "") continue;
                    if (sLine.Substring(0, 1) == "*") continue;

                    switch (sLine)
                    {
                        case "#Modify#":
                            currentMode = 4;
                            continue;
                        default:
                            if (sLine.Substring(0, 1) == "#")
                            {
                                currentMode = -1;
                                continue;
                            }
                            break;
                    }
                    if (currentMode == -1) continue;

                    switch (currentMode)
                    {
                        case 4:
                            {
                                int firstIndex = 0;
                                if (fAdd)
                                {
                                    firstIndex = CurrentLength;
                                    CurrentLength += int.Parse(sLine);
                                }
                                else CurrentLength = int.Parse(sLine);

                                if (CurrentLength > MeshInfo.Length) throw new Exception("Слишком большое количество модицикаций в файле.");

                                int pLen = 0;
                                if (CurrentLength == 0) break;

                                for (int j = firstIndex; j < CurrentLength; j++)
                                {
                                    if (file.EndOfStream) throw new Exception("Неверный формат файла: неожиданный конец файла");
                                    sLine = file.ReadLine().Trim();
                                    MeshInfo[j] = new MeshModify(int.Parse(sLine));

                                    if (file.EndOfStream) throw new Exception("Неверный формат файла: неожиданный конец файла");
                                    sLine = file.ReadLine().Trim();
                                    pLen = int.Parse(sLine);

                                    for (int i = 0; i < pLen; i++)
                                    {
                                        if (file.EndOfStream) throw new Exception("Неверный формат файла: неожиданный конец файла");
                                        sLine = file.ReadLine().Trim();
                                        MeshInfo[j].intParameters[i] = int.Parse(sLine);
                                    }

                                    if (file.EndOfStream) throw new Exception("Неверный формат файла: неожиданный конец файла");
                                    sLine = file.ReadLine().Trim();
                                    pLen = int.Parse(sLine);

                                    for (int i = 0; i < pLen; i++)
                                    {
                                        if (file.EndOfStream) throw new Exception("Неверный формат файла: неожиданный конец файла");
                                        sLine = file.ReadLine().Trim();
                                        MeshInfo[j].floatParameters[i] = float.Parse(sLine, culture);
                                    }
                                }
                            }
                            break;
                    }
                }
            }
            finally
            {
                file.Close();
                file.Dispose();
            }
        }
    }

    public class MeshModify
    {
        public int ModifyCode { get; private set; }
        public int[] intParameters { get; private set; }
        public float[] floatParameters { get; private set; }

        public MeshModify(int code)
        {
            SetCode(code);
        }

        public MeshModify(MeshModify template)
        {
            SetCode(template.ModifyCode);
            switch (ModifyCode)
            {
                case 0: // Смещение по float x,y,z
                    floatParameters[0] = template.floatParameters[0];
                    floatParameters[1] = template.floatParameters[1];
                    floatParameters[2] = template.floatParameters[2];
                    break;
                case 1: // Растяжение по float x,y,z
                    floatParameters[0] = template.floatParameters[0];
                    floatParameters[1] = template.floatParameters[1];
                    floatParameters[2] = template.floatParameters[2];
                    break;
                case 2: // Поворот вокруг оси int (Axe), float (Angle)
                    intParameters[0] = template.intParameters[0];
                    floatParameters[0] = template.floatParameters[0];
                    break;
                case 3: // Выравнивание относительно осей и плоскостей
                    intParameters[0] = template.intParameters[0];
                    break;
                case 4: // Скручивание на углы a1, a2 от плоскости Y1 до Y2
                    intParameters[0] = template.intParameters[0];
                    intParameters[1] = template.intParameters[1];
                    floatParameters[0] = template.floatParameters[0];
                    floatParameters[1] = template.floatParameters[1];
                    floatParameters[2] = template.floatParameters[2];
                    floatParameters[3] = template.floatParameters[3];
                    break;
                case 5: // Изгиб на угол (float) Angle от плоскости (float) Y1 до (float) Y2
                    floatParameters[0] = template.floatParameters[0];
                    floatParameters[1] = template.floatParameters[1];
                    floatParameters[2] = template.floatParameters[2];
                    break;
                case 6: // сплющивание
                    floatParameters[0] = template.floatParameters[0];
                    floatParameters[1] = template.floatParameters[1];
                    floatParameters[2] = template.floatParameters[2];
                    floatParameters[3] = template.floatParameters[3];
                    floatParameters[4] = template.floatParameters[4];
                    floatParameters[5] = template.floatParameters[5];
                    floatParameters[6] = template.floatParameters[6];
                    floatParameters[7] = template.floatParameters[7];
                    break;
                case 7: // сферический градиент
                    intParameters[0] = template.intParameters[0];
                    intParameters[1] = template.intParameters[1];
                    intParameters[2] = template.intParameters[2];
                    intParameters[3] = template.intParameters[3];
                    intParameters[4] = template.intParameters[4];
                    intParameters[5] = template.intParameters[5];
                    intParameters[6] = template.intParameters[6];
                    intParameters[7] = template.intParameters[7];
                    intParameters[8] = template.intParameters[8];

                    floatParameters[0] = template.floatParameters[0];
                    floatParameters[1] = template.floatParameters[1];
                    floatParameters[2] = template.floatParameters[2];
                    floatParameters[3] = template.floatParameters[3];
                    floatParameters[4] = template.floatParameters[4];
                    floatParameters[5] = template.floatParameters[5];
                    floatParameters[6] = template.floatParameters[6];
                    floatParameters[7] = template.floatParameters[7];
                    break;
                default:
                    throw new Exception("MeshModify(" + ModifyCode.ToString() + "): Указан неизвестный тип модификатора");
            }
        }

        public void SetCode(int newCode)
        {
            ModifyCode = newCode;
            switch (ModifyCode)
            {
                case 0: // Смещение по float x,y,z
                    intParameters = null;
                    floatParameters = new float[] { 0, 0, 0 };
                    break;
                case 1: // Растяжение по float x,y,z
                    intParameters = null;
                    floatParameters = new float[] { 100, 100, 100 };
                    break;
                case 2: // Поворот вокруг оси int (Axe), float (Angle)
                    intParameters = new int[] { (int)(Axis3D.OXyz) };
                    floatParameters = new float[] { 0 };
                    break;
                case 3: // Выравнивание относительно осей и плоскостей
                    intParameters = new int[] { 0 }; // тип выравнивания
                    floatParameters = null;
                    break;
                case 4: // Скручивание на углы a1, a2 от плоскости Y1 до Y2, с продолжением вверх/вниз
                    intParameters = new int[] { 0, 0 };
                    floatParameters = new float[] { 0, 0, -50, 50 };
                    break;
                case 5: // Изгиб на угол (float) Angle от плоскости (float) Y1 до (float) Y2
                    intParameters = null;
                    floatParameters = new float[] { 0, 0, 100 };
                    break;
                case 6: // сплющивание
                    intParameters = null;
                    floatParameters = new float[] { -50, 50, -50, 50, -50, 50, 100, 100 };
                    break;
                case 7: // сферический градиент
                    intParameters = new int[] { 0, 0, 0, 0, 0, 255, 255, 255, 0 };
                    floatParameters = new float[] { 0, 0, 0, 0, 100, 100, 100, 100 };
                    break;
                default:
                    throw new Exception("SetCode(" + ModifyCode.ToString() + "): Указан неизвестный тип модификатора");
            }
        }

        public void UpdateListBox(ListBox lv, int index)
        {
            if (index >= lv.Items.Count) throw new Exception("MeshModify.UpdateListBox: указан слишком большой индекс");

            switch (ModifyCode)
            {
                case 0: // Смещение по float x,y,z
                    lv.Items[index] = "Смещение, X: " + floatParameters[0].ToString("0") + ", Y: " + floatParameters[1].ToString("0") + ", Z: " + floatParameters[2].ToString("0");
                    break;
                case 1: // Растяжение по float x,y,z
                    lv.Items[index] = "Растяжение, X: " + floatParameters[0].ToString("0") + "%, Y: " + floatParameters[1].ToString("0") + "%, Z: " + floatParameters[2].ToString("0") + "%";
                    break;
                case 2: // Поворот вокруг оси int (Axe), float (Angle)
                    lv.Items[index] = "Поворот вокруг оси " + AxisName((Axis3D)intParameters[0]) + " на " + (floatParameters[0] / Engine3D.RadianDegrees).ToString("0") + " градусов.";
                    break;
                case 3: // Выравнивание относительно осей и плоскостей
                    switch (intParameters[0])
                    {
                        case 0:
                            lv.Items[index] = "Центрировать XYZ";
                            break;
                        case 1:
                            lv.Items[index] = "Поднять над плоскостью OXZ";
                            break;
                        case 2:
                            lv.Items[index] = "Вправо от плоскости OYZ";
                            break;
                        case 3:
                            lv.Items[index] = "Перед плоскостью OXY";
                            break;
                        default:
                            throw new Exception("MeshModify.UpdateListBox(" + intParameters[0].ToString() + "): Задан некорректный код выравнивания");
                    }
                    break;
                case 4: // Скручивание вокруг оси OY с плоскости float (Y), до плоскости float (Y) на углы от float (Angle) до float (Angle)
                    lv.Items[index] = "Скручивание на (" + (floatParameters[0] / Engine3D.RadianDegrees).ToString("0") + "," + (floatParameters[1] / Engine3D.RadianDegrees).ToString("0") + ") грд., Y: (" + (floatParameters[2]).ToString("0") + "," + (floatParameters[3]).ToString("0") + "); " + ((intParameters[0] == 1) ? "верх;" : "") + ((intParameters[1] == 1) ? "низ;" : "");
                    break;
                case 5: // Изгиб на угол (float) Angle от плоскости (float) Y1 до (float) Y2
                    lv.Items[index] = "Изгиб на " + (floatParameters[0] / Engine3D.RadianDegrees).ToString("0") + " грд., Y: от " + (floatParameters[1]).ToString("0") + " до " + (floatParameters[2]).ToString("0");
                    break;
                case 6: // сплющивание 
                    lv.Items[index] = "Сплющивание " + "(" + floatParameters[0].ToString("0") + "/" + floatParameters[1].ToString("0") + ")-" +
                                                     "(" + floatParameters[2].ToString("0") + "/" + floatParameters[3].ToString("0") + ")-" +
                                                     "(" + floatParameters[4].ToString("0") + "/" + floatParameters[5].ToString("0") + ")-" +
                                                     "(" + floatParameters[6].ToString("0") + ";" + floatParameters[7].ToString("0") + ")";


                    break;
                case 7:
                    lv.Items[index] = "Сферический градиент";
                    break;
                default:
                    throw new Exception("MeshModify.UpdateListBox(" + ModifyCode.ToString() + "): Указан неизвестный тип модификатора");
            }
        }

        public void WriteToListBox(ListBox lv)
        {
            switch (ModifyCode)
            {
                case 0: // Смещение по float x,y,z
                    lv.Items.Add("Смещение, X: " + floatParameters[0].ToString("0") + ", Y: " + floatParameters[1].ToString("0") + ", Z: " + floatParameters[2].ToString("0"));
                    break;
                case 1: // Растяжение по float x,y,z
                    lv.Items.Add("Растяжение, X: " + floatParameters[0].ToString("0") + "%, Y: " + floatParameters[1].ToString("0") + "%, Z: " + floatParameters[2].ToString("0") + "%");
                    break;
                case 2: // Поворот вокруг оси int (Axe), float (Angle)
                    lv.Items.Add("Поворот вокруг оси " + AxisName((Axis3D)intParameters[0]) + " на " + (floatParameters[0] / Engine3D.RadianDegrees).ToString("0") + " градусов.");
                    break;
                case 3: // Выравнивание относительно осей и плоскостей
                    switch (intParameters[0])
                    {
                        case 0:
                            lv.Items.Add("Центрировать XYZ");
                            break;
                        case 1:
                            lv.Items.Add("Поднять над плоскостью OXZ");
                            break;
                        case 2:
                            lv.Items.Add("Вправо от плоскости OYZ");
                            break;
                        case 3:
                            lv.Items.Add("Перед плоскостью OXY");
                            break;
                        default:
                            throw new Exception("MeshModify.WriteToListBox(" + intParameters[0].ToString() + "): Задан некорректный код выравнивания");
                    }
                    break;
                case 4: // Скручивание вокруг оси OY с плоскости float (Y), до плоскости float (Y) на углы от float (Angle) до float (Angle)
                    lv.Items.Add("Скручивание на (" + (floatParameters[0] / Engine3D.RadianDegrees).ToString("0") + "," + (floatParameters[1] / Engine3D.RadianDegrees).ToString("0") + ") грд., Y: (" + (floatParameters[2]).ToString("0") + "," + (floatParameters[3]).ToString("0") + "); " + ((intParameters[0] == 1) ? "верх;" : "") + ((intParameters[1] == 1) ? "низ;" : ""));
                    break;
                case 5: // Изгиб на угол (float) Angle от плоскости (float) Y1 до (float) Y2
                    lv.Items.Add("Изгиб на " + (floatParameters[0] / Engine3D.RadianDegrees).ToString("0") + " грд., Y: от " + (floatParameters[1]).ToString("0") + " до " + (floatParameters[2]).ToString("0"));
                    break;
                case 6: // Сплющивание 
                    lv.Items.Add("Сплющивание " + "(" + floatParameters[0].ToString("0") + "/" + floatParameters[1].ToString("0") + ")-" +
                                                     "(" + floatParameters[2].ToString("0") + "/" + floatParameters[3].ToString("0") + ")-" +
                                                     "(" + floatParameters[4].ToString("0") + "/" + floatParameters[5].ToString("0") + ")-" +
                                                     "(" + floatParameters[6].ToString("0") + ";" + floatParameters[7].ToString("0") + ")");
                    break;
                case 7:
                    lv.Items.Add("Сферический градиент");
                    break;
                default:
                    lv.Items.Add("MeshModify.WriteToListBox(" + ModifyCode.ToString() + "): Указан неизвестный тип модификатора");
                    break;
            }
        }

        protected string AxisName(Axis3D axis)
        {
            switch (axis)
            {
                case Axis3D.OXyz: return "OX";
                case Axis3D.OYxz: return "OY";
                case Axis3D.OZyx: return "OZ";
            }
            return "";
        }
    }

    public class ProjectSaver
    {
        public bool Saving = false;
    }

    public interface IDrawingSurfaceBuilder
    {
        IDrawingSurface Instance();
    }

    public class DrawingSurfaceBuilder: IDrawingSurfaceBuilder
    {
        private PictureBox Picture;
        private int BufferCount;
        public DrawingSurfaceBuilder(PictureBox picture,int bufferCount)
        {
            Picture = picture;
            BufferCount = bufferCount;
        }
        public IDrawingSurface Instance() => new WFDrawingSurface(Picture, BufferCount); 
    }
}
