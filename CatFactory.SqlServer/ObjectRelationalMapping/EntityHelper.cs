﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using CatFactory.ObjectRelationalMapping;
using CatFactory.SqlServer.DocumentObjectModel;

namespace CatFactory.SqlServer.ObjectRelationalMapping
{
    /// <summary>
    /// Contains extension methods to define entities (Tables) using lambda expressions
    /// </summary>
    public static class EntityHelper
    {
#pragma warning disable CS1591
        public static EntityResult<TModel> DefineEntity<TModel>(this Database database, TModel model) where TModel : class
        {
            var result = new EntityResult<TModel>
            {
                Model = model,
                Table = new Table
                {
                    Schema = database.DefaultSchema,
                    Name = model.GetType().Name
                },
                Database = database
            };

            result.Table.ImportBag.ExtendedProperties = new Collection<ExtendedProperty>();

            foreach (var property in model.GetType().GetProperties())
            {
                var propType = property.PropertyType;

                var types = database.GetDatabaseTypeMaps(propType).ToList();

                result.Table.Columns.Add(new Column
                {
                    Name = property.Name,
                    Type = types.Count == 0 ? "" : types.First().DatabaseType
                });

                result.Table.Columns.Last().ImportBag.ExtendedProperties = new Collection<ExtendedProperty>();
            }

            database.Tables.Add(result.Table);

            return result;
        }

        private static string GetPropertyName<TModel, TProperty>(Expression<Func<TModel, TProperty>> selector)
        {
            var memberExpression = selector.Body as MemberExpression;

            if (memberExpression == null)
            {
                if (selector.Body is UnaryExpression unaryExpression)
                    memberExpression = unaryExpression.Operand as MemberExpression;
            }

            return memberExpression.Member.Name;
        }

        private static IEnumerable<string> GetPropertyNames<TModel, TProperty>(Expression<Func<TModel, TProperty>> selector)
        {
            if (selector.Body is NewExpression newExpression)
            {
                foreach (Expression argument in newExpression.Arguments)
                {
                    var prop = argument as MemberExpression;

                    yield return prop.Member.Name;
                }
            }
            else
            {
                var memberExpression = selector.Body as MemberExpression;

                if (memberExpression == null)
                {
                    if (selector.Body is UnaryExpression unaryExpression)
                        memberExpression = unaryExpression.Operand as MemberExpression;
                }

                yield return memberExpression.Member.Name;
            }
        }

        [Obsolete("Use SetNaming method")]
        public static EntityResult<TModel> SetName<TModel>(this EntityResult<TModel> result, string name, string schema = "") where TModel : class
        {
            result.Table.Name = name;
            result.Table.Schema = string.IsNullOrEmpty(schema) ? result.Database.DefaultSchema : schema;

            return result;
        }

        public static EntityResult<TModel> SetNaming<TModel>(this EntityResult<TModel> result, string name, string schema = "") where TModel : class
        {
            result.Table.Name = name;
            result.Table.Schema = string.IsNullOrEmpty(schema) ? result.Database.DefaultSchema : schema;

            return result;
        }

        public static EntityResult<TModel> SetColumnFor<TModel, TProperty>(this EntityResult<TModel> result, Expression<Func<TModel, TProperty>> selector, string type = "", int length = 0, short prec = 0, short scale = 0, bool nullable = false, string collation = "") where TModel : class
        {
            var column = new Column
            {
                Name = GetPropertyName(selector)
            };

            if (result.Table.Columns.Contains(column))
            {
                if (!string.IsNullOrEmpty(type))
                    result.Table[column.Name].Type = type;

                result.Table[column.Name].Length = length;
                result.Table[column.Name].Prec = prec;
                result.Table[column.Name].Scale = scale;
                result.Table[column.Name].Nullable = nullable;

                if (!string.IsNullOrEmpty(collation))
                    result.Table[column.Name].Collation = collation;
            }
            else
            {
                result.Table.Columns.Add(column);
            }

            return result;
        }

        public static EntityResult<TModel> AddExtendedProperty<TModel>(this EntityResult<TModel> result, string name, string value) where TModel : class
        {
            result.Table.ImportBag.ExtendedProperties.Add(new ExtendedProperty(name, value));

            return result;
        }

        public static EntityResult<TModel> AddExtendedProperty<TModel, TProperty>(this EntityResult<TModel> result, Expression<Func<TModel, TProperty>> selector, string name, string value) where TModel : class
        {

            result.Table[GetPropertyName(selector)].ImportBag.ExtendedProperties.Add(new ExtendedProperty(name, value));

            return result;
        }

        public static EntityResult<TModel> SetIdentity<TModel, TProperty>(this EntityResult<TModel> result, Expression<Func<TModel, TProperty>> selector, int seed = 1, int increment = 1) where TModel : class
        {
            result.Table.Identity = new Identity(GetPropertyName(selector), seed, increment);

            return result;
        }

        public static EntityResult<TModel> SetPrimaryKey<TModel, TProperty>(this EntityResult<TModel> result, Expression<Func<TModel, TProperty>> selector, string constraintName = null) where TModel : class
        {
            var names = GetPropertyNames(selector).ToList();

            result.Table.PrimaryKey = new PrimaryKey
            {
                ConstraintName = result.Database.NamingConvention.GetPrimaryKeyConstraintName(result.Table, names.ToArray()),
                Key = names
            };

            return result;
        }

        public static EntityResult<TModel> AddUnique<TModel, TProperty>(this EntityResult<TModel> result, Expression<Func<TModel, TProperty>> selector, string constraintName = null) where TModel : class
        {
            var names = GetPropertyNames(selector).ToList();

            result.Table.Uniques.Add(new Unique
            {
                ConstraintName = string.IsNullOrEmpty(constraintName) ? result.Database.NamingConvention.GetUniqueConstraintName(result.Table, names.ToArray()) : constraintName,
                Key = names
            });

            return result;
        }

        public static EntityResult<TModel> AddForeignKey<TModel, TProperty>(this EntityResult<TModel> result, Expression<Func<TModel, TProperty>> selector, Table table, string constraintName = null) where TModel : class
        {
            var names = GetPropertyNames(selector).ToList();

            result.Table.ForeignKeys.Add(new ForeignKey
            {
                ConstraintName = string.IsNullOrEmpty(constraintName) ? result.Database.NamingConvention.GetForeignKeyConstraintName(result.Table, names.ToArray(), table) : constraintName,
                Key = GetPropertyNames(selector).ToList(),
                References = table.FullName
            });

            return result;
        }

        //public static List<TModel> CreateList<TModel>(this TModel obj) where TModel : class
        //{
        //    return new List<TModel>();
        //}
    }
#pragma warning restore CS1591
}
