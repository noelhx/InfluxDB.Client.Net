﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdysTech.InfluxDB.Client.Net
{
    public interface IInfluxDBClient
    {
        /// <summary>
        /// Creates the specified database
        /// </summary>
        /// <param name="dbName"></param>
        /// <returns>True:success, Fail:Failed to create db</returns>
        Task<bool> CreateDatabaseAsync(string dbName);

        /// <summary>
        /// Queries and Gets list of all existing databases in the Influx server instance
        /// </summary>
        /// <returns>List of DB names, empty list incase of an error</returns>
        Task<List<String>> GetInfluxDBNamesAsync();

        /// <summary>
        /// Gets the whole DB structure for the given databse in Influx.
        /// </summary>
        /// <param name="dbName">Name of the database</param>
        /// <returns>Hierarchical structure, Dictionary<string:measurement, List<field names>></returns>
        Task<Dictionary<string, List<String>>> GetInfluxDBStructureAsync(string dbName);

        /// <summary>
        /// Posts raw write request to Influx.
        /// </summary>
        /// <param name="dbName">Name of the Database</param>
        /// <param name="precision">Unit of the timestamp, Hour->nanosecond</param>
        /// <param name="content">Raw request, as per Line Protocol</param>
        /// <see cref="https://influxdb.com/docs/v0.9/write_protocols/write_syntax.html#http"/>
        /// <returns>true:success, false:failure</returns>
        Task<bool> PostRawValueAsync(string dbName, TimePrecision precision, string content);

        /// <summary>
        /// Posts an InfluxDataPoint to given measurement
        /// </summary>
        /// <param name="dbName">InfluxDB database name</param>
        /// <param name="point">Influx data point to be written</param>
        /// <returns>True:Success, False:Failure</returns>
        ///<exception cref="UnauthorizedAccessException">When Influx needs authentication, and no user name password is supplied or auth fails</exception>
        ///<exception cref="HttpRequestException">all other HTTP exceptions</exception>   
        Task<bool> PostPointAsync(string dbName, IInfluxDatapoint point);

        /// <summary>
        /// Posts series of InfluxDataPoints to given measurement, in batches of 255
        /// </summary>
        /// <param name="dbName">InfluxDB database name</param>
        /// <param name="Points">Collection of Influx data points to be written</param>
        /// <returns>True:Success, False:Failure</returns>
        ///<exception cref="UnauthorizedAccessException">When Influx needs authentication, and no user name password is supplied or auth fails</exception>
        ///<exception cref="HttpRequestException">all other HTTP exceptions</exception>   
         Task<bool> PostPointsAsync(string dbName, IEnumerable<IInfluxDatapoint> points);


        /// <summary>
        /// Queries Influx DB and gets a time series data back. Ideal for fetching measurement values.
        /// The return list is of dynamics, and each element in there will have properties named after columns in series
        /// </summary>
        /// <param name="dbName">Name of the database</param>
        /// <param name="measurementQuery">Query text, Only results with single series are supported for now</param>
        /// <returns>List of ExpandoObjects (in the form of dynamic). 
        /// The objects will have columns as Peoperties with their current values</returns>
        Task<List<dynamic>> QueryDBAsync(string dbName, string measurementQuery);
        
        #region obselete methods
        [Obsolete ("PostValueAsync is deprecated, please use PostDataPointAsync instead.")]
        Task<bool> PostValueAsync(string dbName, string measurement, long timestamp, TimePrecision precision, string tags, string field, double value);
        [Obsolete ("PostValuesAsync is deprecated, please use PostDataPointsAsync instead.")]
        Task<bool> PostValuesAsync(string dbName, string measurement, long timestamp, TimePrecision precision, string tags, IDictionary<string, double> values);
        #endregion
    }
}
