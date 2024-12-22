using AutoCSer.Extensions;
using AutoCSer.ORM;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.BusinessService
{
    /// <summary>
    /// 持久化数据
    /// </summary>
    public sealed partial class Persistence
    {
        /// <summary>
        /// 生成方法序号映射枚举类型代码相对路径
        /// </summary>
        internal const string CommandServerMethodIndexEnumTypePath = @"..\CommonModel\BusinessServiceMethodEnum\";

        /// <summary>
        /// 数据库连接池
        /// </summary>
        private readonly AutoCSer.ORM.ConnectionPool connectionPool;
        /// <summary>
        /// 数据库连接池
        /// </summary>
        internal static AutoCSer.ORM.ConnectionPool ConnectionPool { get { return Instance.connectionPool; } }
        /// <summary>
        /// 持久化数据
        /// </summary>
        /// <param name="connectionPool">数据库连接池</param>
        private Persistence(AutoCSer.ORM.ConnectionPool connectionPool)
        {
            this.connectionPool = connectionPool;
        }

        /// <summary>
        /// 自增ID与其它混合测试模型 数据库表格持久化
        /// </summary>
        private BusinessPersistence<AutoIdentityModel, AutoCSer.TestCase.CommonModel.TableModel.AutoIdentityModel, long> AutoIdentityModel;
        /// <summary>
        /// 自增ID与其它混合测试模型 数据库表格持久化 读取数据操作对象
        /// </summary>
        internal static BusinessQuery<AutoIdentityModel, AutoCSer.TestCase.CommonModel.TableModel.AutoIdentityModel, long> AutoIdentityModelQuery { get { return Instance.AutoIdentityModel.Query; } }
        /// <summary>
        /// 自增ID与其它混合测试模型 数据库表格持久化 写入数据操作对象（写操作对象逻辑分离，用于查找引用搜索写操作逻辑）
        /// </summary>
        internal static BusinessWriter<AutoIdentityModel, AutoCSer.TestCase.CommonModel.TableModel.AutoIdentityModel, long> AutoIdentityModelWriter { get { return Instance.AutoIdentityModel.Writer; } }

        /// <summary>
        /// 字段测试模型 数据库表格持久化
        /// </summary>
        private BusinessPersistence<FieldModel, AutoCSer.TestCase.CommonModel.TableModel.FieldModel, long> FieldModel;
        /// <summary>
        /// 字段测试模型 数据库表格持久化 读取数据操作对象
        /// </summary>
        internal static BusinessQuery<FieldModel, AutoCSer.TestCase.CommonModel.TableModel.FieldModel, long> FieldModelQuery { get { return Instance.FieldModel.Query; } }
        /// <summary>
        /// 字段测试模型 数据库表格持久化 写入数据操作对象（写操作对象逻辑分离，用于查找引用搜索写操作逻辑）
        /// </summary>
        internal static BusinessWriter<FieldModel, AutoCSer.TestCase.CommonModel.TableModel.FieldModel, long> FieldModelWriter { get { return Instance.FieldModel.Writer; } }

        /// <summary>
        /// 属性测试模型 数据库表格持久化
        /// </summary>
        private BusinessPersistence<PropertyModel, AutoCSer.TestCase.CommonModel.TableModel.PropertyModel, string> PropertyModel;
        /// <summary>
        /// 属性测试模型 数据库表格持久化 读取数据操作对象
        /// </summary>
        internal static BusinessQuery<PropertyModel, AutoCSer.TestCase.CommonModel.TableModel.PropertyModel, string> PropertyModelQuery { get { return Instance.PropertyModel.Query; } }
        /// <summary>
        /// 属性测试模型 数据库表格持久化 写入数据操作对象（写操作对象逻辑分离，用于查找引用搜索写操作逻辑）
        /// </summary>
        internal static BusinessWriter<PropertyModel, AutoCSer.TestCase.CommonModel.TableModel.PropertyModel, string> PropertyModelWriter { get { return Instance.PropertyModel.Writer; } }

        /// <summary>
        /// 自定义字段列测试模型 数据库表格持久化
        /// </summary>
        private BusinessPersistence<CustomColumnFieldModel, AutoCSer.TestCase.CommonModel.TableModel.CustomColumnFieldModel, AutoCSer.TestCase.CommonModel.TableModel.CustomColumn.CustomColumnFieldPrimaryKey> CustomColumnFieldModel;
        /// <summary>
        /// 自定义字段列测试模型 数据库表格持久化 读取数据操作对象
        /// </summary>
        internal static BusinessQuery<CustomColumnFieldModel, AutoCSer.TestCase.CommonModel.TableModel.CustomColumnFieldModel, AutoCSer.TestCase.CommonModel.TableModel.CustomColumn.CustomColumnFieldPrimaryKey> CustomColumnFieldModelQuery { get { return Instance.CustomColumnFieldModel.Query; } }
        /// <summary>
        /// 自定义字段列测试模型 数据库表格持久化 写入数据操作对象（写操作对象逻辑分离，用于查找引用搜索写操作逻辑）
        /// </summary>
        internal static BusinessWriter<CustomColumnFieldModel, AutoCSer.TestCase.CommonModel.TableModel.CustomColumnFieldModel, AutoCSer.TestCase.CommonModel.TableModel.CustomColumn.CustomColumnFieldPrimaryKey> CustomColumnFieldModelWriter { get { return Instance.CustomColumnFieldModel.Writer; } }

        /// <summary>
        /// 自定义属性列测试模型 数据库表格持久化
        /// </summary>
        private BusinessPersistence<CustomColumnPropertyModel, AutoCSer.TestCase.CommonModel.TableModel.CustomColumnPropertyModel, AutoCSer.TestCase.CommonModel.TableModel.CustomColumn.CustomColumnPropertyPrimaryKey> CustomColumnPropertyModel;
        /// <summary>
        /// 自定义属性列测试模型 数据库表格持久化 读取数据操作对象
        /// </summary>
        internal static BusinessQuery<CustomColumnPropertyModel, AutoCSer.TestCase.CommonModel.TableModel.CustomColumnPropertyModel, AutoCSer.TestCase.CommonModel.TableModel.CustomColumn.CustomColumnPropertyPrimaryKey> CustomColumnPropertyModelQuery { get { return Instance.CustomColumnPropertyModel.Query; } }
        /// <summary>
        /// 自定义属性列测试模型 数据库表格持久化 写入数据操作对象（写操作对象逻辑分离，用于查找引用搜索写操作逻辑）
        /// </summary>
        internal static BusinessWriter<CustomColumnPropertyModel, AutoCSer.TestCase.CommonModel.TableModel.CustomColumnPropertyModel, AutoCSer.TestCase.CommonModel.TableModel.CustomColumn.CustomColumnPropertyPrimaryKey> CustomColumnPropertyModelWriter { get { return Instance.CustomColumnPropertyModel.Writer; } }

        /// <summary>
        /// 持久化数据初始化访问锁
        /// </summary>
        private static AutoCSer.Threading.SemaphoreSlimLock instanceLock = new Threading.SemaphoreSlimLock(1);
        /// <summary>
        /// 持久化数据
        /// </summary>
        private static Persistence instance;
        /// <summary>
        /// 持久化数据
        /// </summary>
        internal static Persistence Instance
        {
            get
            {
                if (instance != null) return instance;
                LogHelper.ErrorIgnoreException("请在 Main 函数中初始化调用 await Initialize() 避免产生同步阻塞");
                return Initialize().getResult();
            }
        }
        /// <summary>
        /// 获取持久化数据
        /// </summary>
        /// <returns></returns>
        internal static async Task<Persistence> GetInstance()
        {
            if (instance != null) return instance;
            return await Initialize();
        }
        /// <summary>
        /// 初始化持久化数据
        /// </summary>
        /// <returns></returns>
        public static async Task<Persistence> Initialize()
        {
            await instanceLock.EnterAsync();
            try
            {
                if (instance == null)
                {
                    AutoCSer.ORM.ConnectionPool connectionPool =  await AutoCSer.ORM.MSSQL.ConnectionCreator.CreateConnectionPool(@"server=127.0.0.1;database=AutoCSerExample;uid=Example;pwd=Example");
                    Persistence persistence = new Persistence(connectionPool);

                    persistence.AutoIdentityModel = await new AutoCSer.TestCase.CommonModel.BusinessTableEventConsoleOutput<AutoIdentityModel, AutoCSer.TestCase.CommonModel.TableModel.AutoIdentityModel>()
                        .BusinessTableEvent.CreatePersistence<long>(connectionPool);
                    persistence.FieldModel = await new AutoCSer.TestCase.CommonModel.BusinessTableEventConsoleOutput<FieldModel, AutoCSer.TestCase.CommonModel.TableModel.FieldModel>()
                        .BusinessTableEvent.CreatePersistence<long>(connectionPool);
                    persistence.PropertyModel = await new AutoCSer.TestCase.CommonModel.BusinessTableEventConsoleOutput<PropertyModel, AutoCSer.TestCase.CommonModel.TableModel.PropertyModel>()
                        .BusinessTableEvent.CreatePersistence<string>(connectionPool);
                    persistence.CustomColumnFieldModel = await new AutoCSer.TestCase.CommonModel.BusinessTableEventConsoleOutput<CustomColumnFieldModel, AutoCSer.TestCase.CommonModel.TableModel.CustomColumnFieldModel>()
                        .BusinessTableEvent.CreatePersistence<AutoCSer.TestCase.CommonModel.TableModel.CustomColumn.CustomColumnFieldPrimaryKey>(connectionPool);
                    persistence.CustomColumnPropertyModel = await new AutoCSer.TestCase.CommonModel.BusinessTableEventConsoleOutput<CustomColumnPropertyModel, AutoCSer.TestCase.CommonModel.TableModel.CustomColumnPropertyModel>()
                        .BusinessTableEvent.CreatePersistence<AutoCSer.TestCase.CommonModel.TableModel.CustomColumn.CustomColumnPropertyPrimaryKey>(connectionPool);

                    instance = persistence;
                }
            }
            finally { instanceLock.Exit(); }
            return instance;
        }
    }
}
