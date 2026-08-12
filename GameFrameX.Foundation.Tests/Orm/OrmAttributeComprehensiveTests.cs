using System;
using System.Reflection;
using GameFrameX.Foundation.Orm.Attribute;
using Xunit;

namespace GameFrameX.Foundation.Tests.Orm
{
    /// <summary>
    /// Orm.Attribute 全量覆盖测试：构造映射、默认值、null/空边界、AttributeUsage 元数据、反射可应用性、枚举定义。
    /// </summary>
    public class OrmAttributeComprehensiveTests
    {
        // ============================================================
        // 标记型 Attribute（无参构造）
        // ============================================================

        [Fact]
        public void IncrementSeedAttribute_ShouldBeInstantiableWithClassTargetAndSealed()
        {
            var attribute = new IncrementSeedAttribute();

            Assert.NotNull(attribute);
            AssertUsage<IncrementSeedAttribute>(AttributeTargets.Class, false, true);
            Assert.True(typeof(IncrementSeedAttribute).IsSealed);
        }

        [Fact]
        public void IncrementTableAttribute_ShouldBeInstantiableWithClassTargetAndSealed()
        {
            var attribute = new IncrementTableAttribute();

            Assert.NotNull(attribute);
            AssertUsage<IncrementTableAttribute>(AttributeTargets.Class, false, true);
            Assert.True(typeof(IncrementTableAttribute).IsSealed);
        }

        [Fact]
        public void LogTableAttribute_ShouldBeInstantiableWithClassTargetAndSealed()
        {
            var attribute = new LogTableAttribute();

            Assert.NotNull(attribute);
            AssertUsage<LogTableAttribute>(AttributeTargets.Class, false, true);
            Assert.True(typeof(LogTableAttribute).IsSealed);
        }

        [Fact]
        public void SystemTableAttribute_ShouldBeInstantiableWithClassTargetAndSealed()
        {
            var attribute = new SystemTableAttribute();

            Assert.NotNull(attribute);
            AssertUsage<SystemTableAttribute>(AttributeTargets.Class, false, true);
            Assert.True(typeof(SystemTableAttribute).IsSealed);
        }

        [Fact]
        public void TenantTableAttribute_ShouldBeInstantiableWithClassTargetAndSealed()
        {
            var attribute = new TenantTableAttribute();

            Assert.NotNull(attribute);
            AssertUsage<TenantTableAttribute>(AttributeTargets.Class, false, true);
            Assert.True(typeof(TenantTableAttribute).IsSealed);
        }

        // ============================================================
        // AuditTableAttribute
        // ============================================================

        [Fact]
        public void AuditTableAttribute_ShouldExposeDocumentedDefaults()
        {
            var attribute = new AuditTableAttribute();

            Assert.Equal(AuditLevel.ChangesOnly, attribute.AuditLevel);
            Assert.True(attribute.IncludeUserInfo);
            Assert.False(attribute.IncludeIpAddress);
            Assert.Null(attribute.AuditTableName);
            Assert.True(attribute.Enabled);
            AssertUsage<AuditTableAttribute>(AttributeTargets.Class, false, true);
        }

        [Fact]
        public void AuditTableAttribute_PropertiesShouldBeMutable()
        {
            var attribute = new AuditTableAttribute
            {
                AuditLevel = AuditLevel.Full,
                IncludeUserInfo = false,
                IncludeIpAddress = true,
                AuditTableName = "t_audit_user",
                Enabled = false
            };

            Assert.Equal(AuditLevel.Full, attribute.AuditLevel);
            Assert.False(attribute.IncludeUserInfo);
            Assert.True(attribute.IncludeIpAddress);
            Assert.Equal("t_audit_user", attribute.AuditTableName);
            Assert.False(attribute.Enabled);
        }

        // ============================================================
        // ReadOnlyTableAttribute
        // ============================================================

        [Fact]
        public void ReadOnlyTableAttribute_ShouldExposeDocumentedDefaults()
        {
            var attribute = new ReadOnlyTableAttribute();

            Assert.True(attribute.EnableCache);
            Assert.Equal(60, attribute.CacheMinutes);
            Assert.False(attribute.AllowRefresh);
            Assert.Equal(ReadOnlyErrorHandling.ThrowException, attribute.ErrorHandling);
            Assert.Null(attribute.CustomErrorMessage);
            AssertUsage<ReadOnlyTableAttribute>(AttributeTargets.Class, false, true);
        }

        [Fact]
        public void ReadOnlyTableAttribute_PropertiesShouldBeMutable()
        {
            var attribute = new ReadOnlyTableAttribute
            {
                EnableCache = false,
                CacheMinutes = 120,
                AllowRefresh = true,
                ErrorHandling = ReadOnlyErrorHandling.SilentIgnore,
                CustomErrorMessage = "只读"
            };

            Assert.False(attribute.EnableCache);
            Assert.Equal(120, attribute.CacheMinutes);
            Assert.True(attribute.AllowRefresh);
            Assert.Equal(ReadOnlyErrorHandling.SilentIgnore, attribute.ErrorHandling);
            Assert.Equal("只读", attribute.CustomErrorMessage);
        }

        // ============================================================
        // SensitiveFieldAttribute
        // ============================================================

        [Fact]
        public void SensitiveFieldAttribute_ShouldExposeDocumentedDefaults()
        {
            var attribute = new SensitiveFieldAttribute();

            Assert.Equal(MaskingStrategy.None, attribute.Strategy);
            Assert.Equal(0, attribute.KeepPrefix);
            Assert.Equal(0, attribute.KeepSuffix);
            Assert.Null(attribute.CustomPattern);
            AssertUsage<SensitiveFieldAttribute>(AttributeTargets.Property, false, true);
        }

        [Fact]
        public void SensitiveFieldAttribute_PropertiesShouldBeMutable()
        {
            var attribute = new SensitiveFieldAttribute
            {
                Strategy = MaskingStrategy.Phone,
                KeepPrefix = 3,
                KeepSuffix = 4,
                CustomPattern = @"\d{4}"
            };

            Assert.Equal(MaskingStrategy.Phone, attribute.Strategy);
            Assert.Equal(3, attribute.KeepPrefix);
            Assert.Equal(4, attribute.KeepSuffix);
            Assert.Equal(@"\d{4}", attribute.CustomPattern);
        }

        // ============================================================
        // CacheTableAttribute
        // ============================================================

        [Fact]
        public void CacheTableAttribute_ShouldStoreCacheTypeAndExposeDefaults()
        {
            var attribute = new CacheTableAttribute("Redis");

            Assert.Equal("Redis", attribute.CacheType);
            Assert.Equal(30, attribute.ExpireMinutes);
            Assert.Null(attribute.KeyPrefix);
            Assert.True(attribute.Enabled);
            AssertUsage<CacheTableAttribute>(AttributeTargets.Class, false, true);
        }

        [Fact]
        public void CacheTableAttribute_PropertiesShouldBeMutable()
        {
            var attribute = new CacheTableAttribute("Redis")
            {
                ExpireMinutes = 90,
                KeyPrefix = "user:",
                Enabled = false
            };

            Assert.Equal(90, attribute.ExpireMinutes);
            Assert.Equal("user:", attribute.KeyPrefix);
            Assert.False(attribute.Enabled);
        }

        [Fact]
        public void CacheTableAttribute_WithNullCacheType_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new CacheTableAttribute(null!));
        }

        // ============================================================
        // ConstAttribute / CustomUnifyResultAttribute（均 All + AllowMultiple）
        // ============================================================

        [Fact]
        public void ConstAttribute_ShouldStoreNameAndAllowMultipleOnAllTargets()
        {
            var attribute = new ConstAttribute("StatusCode");

            Assert.Equal("StatusCode", attribute.Name);
            AssertUsage<ConstAttribute>(AttributeTargets.All, true, true);
        }

        [Fact]
        public void ConstAttribute_WithNullName_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ConstAttribute(null!));
        }

        [Fact]
        public void CustomUnifyResultAttribute_ShouldStoreNameAndAllowMultipleOnAllTargets()
        {
            var attribute = new CustomUnifyResultAttribute("BizResult");

            Assert.Equal("BizResult", attribute.Name);
            AssertUsage<CustomUnifyResultAttribute>(AttributeTargets.All, true, true);
        }

        [Fact]
        public void CustomUnifyResultAttribute_WithNullName_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new CustomUnifyResultAttribute(null!));
        }

        // ============================================================
        // DataScopeAttribute
        // ============================================================

        [Fact]
        public void DataScopeAttribute_ShouldStoreScopeFieldAndExposeDefaults()
        {
            var attribute = new DataScopeAttribute("DataScope");

            Assert.Equal("DataScope", attribute.ScopeField);
            Assert.Equal(DataScope.Self, attribute.DefaultScope);
            Assert.Null(attribute.DepartmentField);
            AssertUsage<DataScopeAttribute>(AttributeTargets.Class, false, true);
        }

        [Fact]
        public void DataScopeAttribute_PropertiesShouldBeMutable()
        {
            var attribute = new DataScopeAttribute("DataScope")
            {
                DefaultScope = DataScope.DepartmentAndChild,
                DepartmentField = "DeptId"
            };

            Assert.Equal(DataScope.DepartmentAndChild, attribute.DefaultScope);
            Assert.Equal("DeptId", attribute.DepartmentField);
        }

        [Fact]
        public void DataScopeAttribute_WithNullScopeField_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new DataScopeAttribute(null!));
        }

        // ============================================================
        // EntityIndexAttribute（使用 ArgumentException.ThrowIfNullOrWhiteSpace）
        // ============================================================

        [Fact]
        public void EntityIndexAttribute_ShouldStoreNameAndExposeDefaults()
        {
            var attribute = new EntityIndexAttribute("IX_User_Email");

            Assert.Equal("IX_User_Email", attribute.Name);
            Assert.False(attribute.Unique);
            Assert.True(attribute.IsAscending);
            AssertUsage<EntityIndexAttribute>(AttributeTargets.Property, false, true);
        }

        [Fact]
        public void EntityIndexAttribute_PropertiesShouldBeMutable()
        {
            var attribute = new EntityIndexAttribute("IX")
            {
                Unique = true,
                IsAscending = false
            };

            Assert.True(attribute.Unique);
            Assert.False(attribute.IsAscending);
        }

        [Fact]
        public void EntityIndexAttribute_WithNullName_ShouldThrowArgumentNullException()
        {
            // ArgumentException.ThrowIfNullOrWhiteSpace 对 null 抛 ArgumentNullException
            Assert.Throws<ArgumentNullException>(() => new EntityIndexAttribute(null!));
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void EntityIndexAttribute_WithEmptyOrWhiteSpaceName_ShouldThrowArgumentException(string name)
        {
            // 空串与空白串抛（非 ArgumentNull 的）ArgumentException
            Assert.Throws<ArgumentException>(() => new EntityIndexAttribute(name));
        }

        // ============================================================
        // ImportExportAttribute
        // ============================================================

        [Fact]
        public void ImportExportAttribute_Parameterless_ShouldExposeDefaults()
        {
            var attribute = new ImportExportAttribute();

            Assert.True(attribute.ImportEnabled);
            Assert.True(attribute.ExportEnabled);
            Assert.Null(attribute.SheetName);
            Assert.Null(attribute.DisplayName);
            Assert.Equal(0, attribute.Order);
            Assert.False(attribute.IgnoreImport);
            Assert.False(attribute.IgnoreExport);
            AssertUsage<ImportExportAttribute>(AttributeTargets.Class | AttributeTargets.Property, false, true);
        }

        [Fact]
        public void ImportExportAttribute_WithDisplayName_ShouldStoreIt()
        {
            var attribute = new ImportExportAttribute("订单编号");

            Assert.Equal("订单编号", attribute.DisplayName);
        }

        [Fact]
        public void ImportExportAttribute_PropertiesShouldBeMutable()
        {
            var attribute = new ImportExportAttribute
            {
                ImportEnabled = false,
                ExportEnabled = false,
                SheetName = "订单",
                Order = 5,
                IgnoreImport = true,
                IgnoreExport = true
            };

            Assert.False(attribute.ImportEnabled);
            Assert.False(attribute.ExportEnabled);
            Assert.Equal("订单", attribute.SheetName);
            Assert.Equal(5, attribute.Order);
            Assert.True(attribute.IgnoreImport);
            Assert.True(attribute.IgnoreExport);
        }

        [Fact]
        public void ImportExportAttribute_WithNullDisplayName_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ImportExportAttribute(null!));
        }

        // ============================================================
        // PartitionTableAttribute（3 个重载）
        // ============================================================

        [Fact]
        public void PartitionTableAttribute_TwoArgCtor_ShouldStoreKeyAndType()
        {
            var attribute = new PartitionTableAttribute("CreateTime", PartitionType.Range);

            Assert.Equal("CreateTime", attribute.PartitionKey);
            Assert.Equal(PartitionType.Range, attribute.PartitionType);
            Assert.Equal(PartitionInterval.Monthly, attribute.Interval);
            Assert.Equal(4, attribute.PartitionCount);
            Assert.Null(attribute.PartitionValues);
            Assert.True(attribute.AutoCreatePartition);
            Assert.Equal(0, attribute.RetentionDays);
            AssertUsage<PartitionTableAttribute>(AttributeTargets.Class, false, true);
        }

        [Fact]
        public void PartitionTableAttribute_WithIntervalCtor_ShouldStoreInterval()
        {
            var attribute = new PartitionTableAttribute("CreateTime", PartitionType.Range, PartitionInterval.Daily);

            Assert.Equal(PartitionInterval.Daily, attribute.Interval);
        }

        [Fact]
        public void PartitionTableAttribute_WithPartitionCountCtor_ShouldStoreCount()
        {
            var attribute = new PartitionTableAttribute("UserId", PartitionType.Hash, 8);

            Assert.Equal(PartitionType.Hash, attribute.PartitionType);
            Assert.Equal(8, attribute.PartitionCount);
        }

        [Fact]
        public void PartitionTableAttribute_PropertiesShouldBeMutable()
        {
            var attribute = new PartitionTableAttribute("CreateTime", PartitionType.List)
            {
                Interval = PartitionInterval.Yearly,
                PartitionValues = "2024,2025",
                AutoCreatePartition = false,
                RetentionDays = 30
            };

            Assert.Equal(PartitionInterval.Yearly, attribute.Interval);
            Assert.Equal("2024,2025", attribute.PartitionValues);
            Assert.False(attribute.AutoCreatePartition);
            Assert.Equal(30, attribute.RetentionDays);
        }

        [Theory]
        [InlineData(PartitionType.Range)]
        [InlineData(PartitionType.List)]
        [InlineData(PartitionType.Hash)]
        public void PartitionTableAttribute_WithNullPartitionKey_ShouldThrowArgumentNullException(PartitionType type)
        {
            Assert.Throws<ArgumentNullException>(() => new PartitionTableAttribute(null!, type));
            Assert.Throws<ArgumentNullException>(() => new PartitionTableAttribute(null!, type, PartitionInterval.Daily));
            Assert.Throws<ArgumentNullException>(() => new PartitionTableAttribute(null!, type, 4));
        }

        // ============================================================
        // SoftDeleteAttribute（3 个重载，补充现有测试）
        // ============================================================

        [Fact]
        public void SoftDeleteAttribute_SingleArgCtor_ShouldStoreFieldWithDefaults()
        {
            var attribute = new SoftDeleteAttribute("IsDeleted");

            Assert.Equal("IsDeleted", attribute.DeletedField);
            Assert.Null(attribute.DeletedTimeField);
            Assert.Null(attribute.DeletedByField);
            Assert.True((bool)attribute.DeletedValue);
            Assert.False((bool)attribute.NotDeletedValue);
            Assert.True(attribute.AutoFilter);
            AssertUsage<SoftDeleteAttribute>(AttributeTargets.Class, false, true);
        }

        [Fact]
        public void SoftDeleteAttribute_TwoArgCtor_ShouldStoreTimeField()
        {
            var attribute = new SoftDeleteAttribute("IsDeleted", "DeletedTime");

            Assert.Equal("IsDeleted", attribute.DeletedField);
            Assert.Equal("DeletedTime", attribute.DeletedTimeField);
            Assert.Null(attribute.DeletedByField);
        }

        [Fact]
        public void SoftDeleteAttribute_CustomDeletedValue_ShouldAcceptArbitraryObject()
        {
            var attribute = new SoftDeleteAttribute("IsDeleted")
            {
                DeletedValue = 1,
                NotDeletedValue = 0,
                AutoFilter = false
            };

            Assert.Equal(1, attribute.DeletedValue);
            Assert.Equal(0, attribute.NotDeletedValue);
            Assert.False(attribute.AutoFilter);
        }

        [Fact]
        public void SoftDeleteAttribute_WithNullDeletedField_ShouldThrowForAllOverloads()
        {
            Assert.Throws<ArgumentNullException>(() => new SoftDeleteAttribute(null!));
            Assert.Throws<ArgumentNullException>(() => new SoftDeleteAttribute(null!, "DeletedTime"));
            Assert.Throws<ArgumentNullException>(() => new SoftDeleteAttribute(null!, "DeletedTime", "DeletedBy"));
        }

        // ============================================================
        // TreeEntityAttribute
        // ============================================================

        [Fact]
        public void TreeEntityAttribute_ShouldStoreParentIdFieldAndExposeDefaults()
        {
            var attribute = new TreeEntityAttribute("ParentId");

            Assert.Equal("ParentId", attribute.ParentIdField);
            Assert.Null(attribute.PathField);
            Assert.Null(attribute.LevelField);
            Assert.Null(attribute.SortField);
            Assert.Null(attribute.ChildrenField);
            Assert.Equal(0, attribute.MaxDepth);
            AssertUsage<TreeEntityAttribute>(AttributeTargets.Class, false, true);
        }

        [Fact]
        public void TreeEntityAttribute_PropertiesShouldBeMutable()
        {
            var attribute = new TreeEntityAttribute("ParentId")
            {
                PathField = "Path",
                LevelField = "Level",
                SortField = "Sort",
                ChildrenField = "Children",
                MaxDepth = 5
            };

            Assert.Equal("Path", attribute.PathField);
            Assert.Equal("Level", attribute.LevelField);
            Assert.Equal("Sort", attribute.SortField);
            Assert.Equal("Children", attribute.ChildrenField);
            Assert.Equal(5, attribute.MaxDepth);
        }

        [Fact]
        public void TreeEntityAttribute_WithNullParentIdField_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new TreeEntityAttribute(null!));
        }

        // ============================================================
        // VersionControlAttribute（2 个重载）
        // ============================================================

        [Fact]
        public void VersionControlAttribute_SingleArgCtor_ShouldStoreVersionFieldWithDefaults()
        {
            var attribute = new VersionControlAttribute("RowVersion");

            Assert.Equal("RowVersion", attribute.VersionField);
            Assert.Equal(VersionStrategy.Optimistic, attribute.Strategy);
            Assert.False(attribute.EnableHistory);
            Assert.Null(attribute.HistoryTableName);
            Assert.Equal(0, attribute.MaxHistoryVersions);
            Assert.Equal(ConflictResolution.ThrowException, attribute.ConflictHandling);
            Assert.True(attribute.AutoUpdateVersion);
            Assert.Equal(VersionComparison.Increment, attribute.ComparisonMode);
            AssertUsage<VersionControlAttribute>(AttributeTargets.Class, false, true);
        }

        [Fact]
        public void VersionControlAttribute_TwoArgCtor_ShouldStoreStrategy()
        {
            var attribute = new VersionControlAttribute("RowVersion", VersionStrategy.Pessimistic);

            Assert.Equal(VersionStrategy.Pessimistic, attribute.Strategy);
        }

        [Fact]
        public void VersionControlAttribute_PropertiesShouldBeMutable()
        {
            var attribute = new VersionControlAttribute("RowVersion")
            {
                EnableHistory = true,
                HistoryTableName = "t_version_history",
                MaxHistoryVersions = 10,
                ConflictHandling = ConflictResolution.AutoRetry,
                AutoUpdateVersion = false,
                ComparisonMode = VersionComparison.Timestamp
            };

            Assert.True(attribute.EnableHistory);
            Assert.Equal("t_version_history", attribute.HistoryTableName);
            Assert.Equal(10, attribute.MaxHistoryVersions);
            Assert.Equal(ConflictResolution.AutoRetry, attribute.ConflictHandling);
            Assert.False(attribute.AutoUpdateVersion);
            Assert.Equal(VersionComparison.Timestamp, attribute.ComparisonMode);
        }

        [Fact]
        public void VersionControlAttribute_WithNullVersionField_ShouldThrowForAllOverloads()
        {
            Assert.Throws<ArgumentNullException>(() => new VersionControlAttribute(null!));
            Assert.Throws<ArgumentNullException>(() => new VersionControlAttribute(null!, VersionStrategy.Hash));
        }

        // ============================================================
        // 枚举定义与取值
        // ============================================================

        [Theory]
        [InlineData(AuditLevel.None, 0)]
        [InlineData(AuditLevel.ChangesOnly, 1)]
        [InlineData(AuditLevel.Full, 2)]
        [InlineData(AuditLevel.Custom, 3)]
        public void AuditLevel_ShouldHaveDocumentedValues(AuditLevel value, int expected)
        {
            Assert.Equal(expected, (int)value);
        }

        [Theory]
        [InlineData(DataScope.All, 0)]
        [InlineData(DataScope.Department, 1)]
        [InlineData(DataScope.DepartmentAndChild, 2)]
        [InlineData(DataScope.Self, 3)]
        [InlineData(DataScope.Custom, 4)]
        public void DataScope_ShouldHaveDocumentedValues(DataScope value, int expected)
        {
            Assert.Equal(expected, (int)value);
        }

        [Theory]
        [InlineData(PartitionType.Range, 0)]
        [InlineData(PartitionType.List, 1)]
        [InlineData(PartitionType.Hash, 2)]
        [InlineData(PartitionType.Composite, 3)]
        public void PartitionType_ShouldHaveDocumentedValues(PartitionType value, int expected)
        {
            Assert.Equal(expected, (int)value);
        }

        [Theory]
        [InlineData(PartitionInterval.Daily, 0)]
        [InlineData(PartitionInterval.Weekly, 1)]
        [InlineData(PartitionInterval.Monthly, 2)]
        [InlineData(PartitionInterval.Quarterly, 3)]
        [InlineData(PartitionInterval.Yearly, 4)]
        [InlineData(PartitionInterval.Custom, 5)]
        public void PartitionInterval_ShouldHaveDocumentedValues(PartitionInterval value, int expected)
        {
            Assert.Equal(expected, (int)value);
        }

        [Theory]
        [InlineData(ReadOnlyErrorHandling.ThrowException, 0)]
        [InlineData(ReadOnlyErrorHandling.SilentIgnore, 1)]
        [InlineData(ReadOnlyErrorHandling.LogWarning, 2)]
        [InlineData(ReadOnlyErrorHandling.Custom, 3)]
        public void ReadOnlyErrorHandling_ShouldHaveDocumentedValues(ReadOnlyErrorHandling value, int expected)
        {
            Assert.Equal(expected, (int)value);
        }

        [Theory]
        [InlineData(MaskingStrategy.None, 0)]
        [InlineData(MaskingStrategy.Phone, 1)]
        [InlineData(MaskingStrategy.IdCard, 2)]
        [InlineData(MaskingStrategy.Email, 3)]
        [InlineData(MaskingStrategy.BankCard, 4)]
        [InlineData(MaskingStrategy.Name, 5)]
        [InlineData(MaskingStrategy.Address, 6)]
        [InlineData(MaskingStrategy.Custom, 7)]
        public void MaskingStrategy_ShouldHaveDocumentedValues(MaskingStrategy value, int expected)
        {
            Assert.Equal(expected, (int)value);
        }

        [Theory]
        [InlineData(VersionStrategy.Optimistic, 0)]
        [InlineData(VersionStrategy.Pessimistic, 1)]
        [InlineData(VersionStrategy.Timestamp, 2)]
        [InlineData(VersionStrategy.LastModified, 3)]
        [InlineData(VersionStrategy.Hash, 4)]
        [InlineData(VersionStrategy.Custom, 5)]
        public void VersionStrategy_ShouldHaveDocumentedValues(VersionStrategy value, int expected)
        {
            Assert.Equal(expected, (int)value);
        }

        [Theory]
        [InlineData(ConflictResolution.ThrowException, 0)]
        [InlineData(ConflictResolution.AutoRetry, 1)]
        [InlineData(ConflictResolution.ForceOverwrite, 2)]
        [InlineData(ConflictResolution.MergeChanges, 3)]
        [InlineData(ConflictResolution.UserChoice, 4)]
        [InlineData(ConflictResolution.Custom, 5)]
        public void ConflictResolution_ShouldHaveDocumentedValues(ConflictResolution value, int expected)
        {
            Assert.Equal(expected, (int)value);
        }

        [Theory]
        [InlineData(VersionComparison.Increment, 0)]
        [InlineData(VersionComparison.Timestamp, 1)]
        [InlineData(VersionComparison.String, 2)]
        [InlineData(VersionComparison.Custom, 3)]
        public void VersionComparison_ShouldHaveDocumentedValues(VersionComparison value, int expected)
        {
            Assert.Equal(expected, (int)value);
        }

        // ============================================================
        // 反射可应用性：验证 Attribute 真的能附着到目标并读取
        // ============================================================

        [Fact]
        public void Attributes_ShouldBeRetrievableViaReflectionOnAnnotatedType()
        {
            var type = typeof(AnnotatedEntity);

            // 类级 Attribute
            Assert.NotNull(type.GetCustomAttribute<AuditTableAttribute>());
            Assert.NotNull(type.GetCustomAttribute<IncrementSeedAttribute>());
            Assert.NotNull(type.GetCustomAttribute<TenantTableAttribute>());
            Assert.NotNull(type.GetCustomAttribute<SoftDeleteAttribute>());
            Assert.NotNull(type.GetCustomAttribute<PartitionTableAttribute>());

            // 属性级 Attribute
            var property = type.GetProperty(nameof(AnnotatedEntity.Code))!;
            var indexAttr = property.GetCustomAttribute<EntityIndexAttribute>();
            var sensitiveAttr = property.GetCustomAttribute<SensitiveFieldAttribute>();
            var importExportAttr = property.GetCustomAttribute<ImportExportAttribute>();

            Assert.NotNull(indexAttr);
            Assert.True(indexAttr!.Unique);
            Assert.Equal("IX_Code", indexAttr.Name);

            Assert.NotNull(sensitiveAttr);
            Assert.Equal(MaskingStrategy.Phone, sensitiveAttr!.Strategy);
            Assert.Equal(3, sensitiveAttr.KeepPrefix);

            Assert.NotNull(importExportAttr);
            Assert.Equal("编码", importExportAttr!.DisplayName);
            Assert.Equal(1, importExportAttr.Order);
        }

        [Fact]
        public void ConstAttribute_ShouldBeApplicableMultipleTimesViaReflection()
        {
            var type = typeof(MultiConstEntity);

            var consts = type.GetCustomAttributes<ConstAttribute>();
            Assert.Equal(2, consts.Count());
            Assert.Contains(consts, a => a.Name == "A");
            Assert.Contains(consts, a => a.Name == "B");
        }

        // ============================================================
        // 补充测试:EntityIndexAttribute Theory 默认值与可写性组合
        // (任务描述提到 Fields/IndexType,源码实际无此属性,按真实源码补充)
        // ============================================================

        [Theory]
        [InlineData("IX_User_Email")]
        [InlineData("IX_Order_CreateTime")]
        [InlineData("IX_包含中文")]
        public void EntityIndexAttribute_WithVariousNames_ShouldStoreNameAndDefaults(string name)
        {
            // Arrange & Act
            var attribute = new EntityIndexAttribute(name);

            // Assert
            Assert.Equal(name, attribute.Name);
            Assert.False(attribute.Unique);
            Assert.True(attribute.IsAscending);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public void EntityIndexAttribute_UniqueAndIsAscending_ShouldBeMutableAndRoundTrip(bool unique, bool isAscending)
        {
            // Arrange
            var attribute = new EntityIndexAttribute("IX");

            // Act
            attribute.Unique = unique;
            attribute.IsAscending = isAscending;

            // Assert
            Assert.Equal(unique, attribute.Unique);
            Assert.Equal(isAscending, attribute.IsAscending);
        }

        // ============================================================
        // 补充测试:CacheTableAttribute 无参构造、自定义 expireMinutes、可写性
        // ============================================================

        [Fact]
        public void CacheTableAttribute_ParameterlessCtor_ShouldApplyAllDefaults()
        {
            // Arrange & Act
            var attribute = new CacheTableAttribute();

            // Assert
            Assert.Equal("Memory", attribute.CacheType);
            Assert.Equal(30, attribute.ExpireMinutes);
            Assert.Null(attribute.KeyPrefix);
            Assert.True(attribute.Enabled);
        }

        [Theory]
        [InlineData("Redis", 30)]          // 默认 expireMinutes
        [InlineData("Redis", 90)]
        [InlineData("Memory", 10)]
        [InlineData("Distributed", 1440)]
        public void CacheTableAttribute_TwoArgCtor_ShouldStoreCacheTypeAndExpireMinutes(string cacheType, int expireMinutes)
        {
            // Arrange & Act
            var attribute = new CacheTableAttribute(cacheType, expireMinutes);

            // Assert
            Assert.Equal(cacheType, attribute.CacheType);
            Assert.Equal(expireMinutes, attribute.ExpireMinutes);
            Assert.Null(attribute.KeyPrefix);
            Assert.True(attribute.Enabled);
        }

        [Theory]
        [InlineData(0)]                    // 0:立即过期(语义上有效)
        [InlineData(-1)]                   // 负数:源码当前容忍(无 guard)
        [InlineData(int.MaxValue)]
        public void CacheTableAttribute_ExpireMinutes_ShouldAcceptAnyInt_AsCurrentDesign(int minutes)
        {
            // Arrange
            var attribute = new CacheTableAttribute("Memory");

            // Act
            attribute.ExpireMinutes = minutes;

            // Assert
            Assert.Equal(minutes, attribute.ExpireMinutes);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("user:")]
        [InlineData("game:order:")]
        public void CacheTableAttribute_KeyPrefix_ShouldAcceptNullOrString_AsCurrentDesign(string? prefix)
        {
            // Arrange
            var attribute = new CacheTableAttribute("Redis");

            // Act
            attribute.KeyPrefix = prefix;

            // Assert
            Assert.Equal(prefix, attribute.KeyPrefix);
        }

        [Fact]
        public void CacheTableAttribute_Enabled_ShouldToggleBothDirections()
        {
            // Arrange
            var attribute = new CacheTableAttribute("Redis");

            // Assert - 默认 true
            Assert.True(attribute.Enabled);

            // Act & Assert - 关闭
            attribute.Enabled = false;
            Assert.False(attribute.Enabled);

            // Act & Assert - 重新开启
            attribute.Enabled = true;
            Assert.True(attribute.Enabled);
        }

        // ============================================================
        // 补充测试:ReadOnlyTableAttribute(bool, int) 构造重载
        // ============================================================

        [Theory]
        [InlineData(true, 60)]            // 与无参默认一致
        [InlineData(true, 120)]
        [InlineData(false, 0)]
        [InlineData(false, 30)]
        public void ReadOnlyTableAttribute_TwoArgCtor_ShouldStoreEnableCacheAndCacheMinutes(bool enableCache, int cacheMinutes)
        {
            // Arrange & Act
            var attribute = new ReadOnlyTableAttribute(enableCache, cacheMinutes);

            // Assert - ctor 入参
            Assert.Equal(enableCache, attribute.EnableCache);
            Assert.Equal(cacheMinutes, attribute.CacheMinutes);

            // Assert - 其他属性仍为默认值
            Assert.False(attribute.AllowRefresh);
            Assert.Equal(ReadOnlyErrorHandling.ThrowException, attribute.ErrorHandling);
            Assert.Null(attribute.CustomErrorMessage);
        }

        [Fact]
        public void ReadOnlyTableAttribute_TwoArgCtor_DefaultCacheMinutesIs60()
        {
            // Arrange & Act - 仅传 enableCache,cacheMinutes 走默认值 60
            var attribute = new ReadOnlyTableAttribute(true);

            // Assert
            Assert.True(attribute.EnableCache);
            Assert.Equal(60, attribute.CacheMinutes);
        }

        // ============================================================
        // 补充测试:AuditTableAttribute(AuditLevel) 构造重载
        // ============================================================

        [Theory]
        [InlineData(AuditLevel.None)]
        [InlineData(AuditLevel.ChangesOnly)]
        [InlineData(AuditLevel.Full)]
        [InlineData(AuditLevel.Custom)]
        public void AuditTableAttribute_SingleArgCtor_ShouldStoreAuditLevel_AndKeepOtherDefaults(AuditLevel auditLevel)
        {
            // Arrange & Act
            var attribute = new AuditTableAttribute(auditLevel);

            // Assert - ctor 入参
            Assert.Equal(auditLevel, attribute.AuditLevel);

            // Assert - 其他属性仍为默认值
            Assert.True(attribute.IncludeUserInfo);
            Assert.False(attribute.IncludeIpAddress);
            Assert.Null(attribute.AuditTableName);
            Assert.True(attribute.Enabled);
        }

        // ============================================================
        // 测试辅助
        // ============================================================

        private static void AssertUsage<TAttribute>(AttributeTargets expectedOn, bool allowMultiple, bool inherited)
            where TAttribute : Attribute
        {
            var usage = typeof(TAttribute).GetCustomAttribute<AttributeUsageAttribute>();

            Assert.NotNull(usage);
            Assert.Equal(expectedOn, usage!.ValidOn);
            Assert.Equal(allowMultiple, usage.AllowMultiple);
            Assert.Equal(inherited, usage.Inherited);
        }

        // 带完整注解的测试实体
        [AuditTable]
        [IncrementSeed]
        [TenantTable]
        [SoftDelete("IsDeleted")]
        [PartitionTable("CreateTime", PartitionType.Range)]
        private sealed class AnnotatedEntity
        {
            [EntityIndex("IX_Code", Unique = true)]
            [SensitiveField(Strategy = MaskingStrategy.Phone, KeepPrefix = 3)]
            [ImportExport("编码", Order = 1)]
            public string Code { get; set; } = string.Empty;
        }

        [Const("A")]
        [Const("B")]
        private sealed class MultiConstEntity
        {
        }
    }
}
