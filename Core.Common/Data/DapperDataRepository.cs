using Core.Common.Configuration;
using Dapper;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace Core.Common.Data {
    public class DapperDataRepository {
        protected SqlConnection db;
        protected string _connectionName;

        public DapperDataRepository(string connectionName) {
            _connectionName = connectionName;
        }

        protected SqlConnection GetConnection(string connectionName = null) {
            return new SqlConnection(ConfigurationManager.ConnectionStrings[connectionName ?? _connectionName].ConnectionString);
        }

        public IEnumerable<E> Query<E>(string query, object param = null, string connectionName = null) {
            using (var db = GetConnection(connectionName)) {
                return db.Query<E>(query, param, commandTimeout: 5000000);
            }
        }

        public E QueryFirst<E>(string query, object param = null, string connectionName = null) {
            using (var db = GetConnection(connectionName)) {
                return db.QueryFirstOrDefault<E>(query, param, commandTimeout: 5000000);
            }
        }

        public SqlMapper.GridReader QueryMultiple(string query, object param = null, string connectionName = null) {
            var db = GetConnection(connectionName);
            return db.QueryMultiple(query, param, commandTimeout: 5000000);
        }

        public int Execute(string sql, object param = null, string connectionName = null) {
            using (var db = GetConnection(connectionName)) {
                return db.Execute(sql, param, commandTimeout: 5000000);
            }
        }

        public E ExecuteScalar<E>(string sql, object param = null, string connectionName = null) {
            using (var db = GetConnection(connectionName)) {
                return db.ExecuteScalar<E>(sql, param, commandTimeout: 5000000);
            }
        }

        public void BulkInsert<E>(IEnumerable<E> data, string tableName, string connectionName = null, List<string> excludedProperties = null) {
            using (SqlConnection connection = GetConnection(connectionName)) {
                SqlBulkCopy bulkCopy =
                    new SqlBulkCopy
                    (
                    connection,
                    SqlBulkCopyOptions.TableLock |
                    SqlBulkCopyOptions.FireTriggers |
                    SqlBulkCopyOptions.UseInternalTransaction,
                    null
                    );
                // set the destination table name
                bulkCopy.DestinationTableName = tableName;
                connection.Open();
                // write the data in the "dataTable"
                var table = ToDataTable(data, excludedProperties);
                foreach (DataColumn col in table.Columns) {
                    bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                }
                bulkCopy.WriteToServer(table);
                connection.Close();
            }
        }

        public DataTable ToDataTable<E>(IEnumerable<E> data, List<string> excludedProperties = null) {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(E));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties) {
                if (excludedProperties != null && excludedProperties.Contains(prop.Name))
                    continue;
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }
            foreach (E item in data) {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties) {
                    if (excludedProperties != null && excludedProperties.Contains(prop.Name))
                        continue;
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                table.Rows.Add(row);
            }
            return table;
        }
    }
}
