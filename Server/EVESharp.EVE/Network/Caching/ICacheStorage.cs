using System;
using System.Collections.Generic;
using EVESharp.Types;
using EVESharp.Types.Collections;

namespace EVESharp.EVE.Network.Caching;

public interface ICacheStorage
{
    /// <summary>
    /// Checks if a cached object already exists
    /// </summary>
    /// <param name="name">The object to look for</param>
    bool Exists (string name);

    /// <summary>
    /// Checks if a cached object for the given <paramref name="service"/> and <paramref name="method" /> already exists
    /// </summary>
    /// <param name="service">The service that generates the cache</param>
    /// <param name="method">The method that generates the cache</param>
    bool Exists (string service, string method);

    /// <summary>
    /// Gets the specified cached content if it exists. This content is ready to be sent to the EVE Client
    /// </summary>
    /// <param name="name">The cached content to look for</param>
    PyDataType Get (string name);

    /// <summary>
    /// Gets the specified cached content if it exists. The content is ready to be sent to the EVE Client 
    /// </summary>
    /// <param name="service">The service that generated the cached content</param>
    /// <param name="method">The method that generated the cached content</param>
    PyDataType Get (string service, string method);

    /// <summary>
    /// Saves the given <paramref name="data"/> into the cache storage and identifies it by the given <paramref name="name"/>
    /// </summary>
    /// <param name="name">The name to identify the cached data by</param>
    /// <param name="data">The data to cache</param>
    /// <param name="timestamp">The timestamp of when the cached object was created</param>
    void Store (string name, PyDataType data, long timestamp);

    /// <summary>
    /// Saves the given <paramref name="data"/> into the cache storage and identifies it by the <paramref name="service"/> and <paramref name="method"/> that generated it
    /// </summary>
    /// <param name="service">The service that generated the cached object</param>
    /// <param name="method">The method that generated the cached object</param>
    /// <param name="data">The data to cache</param>
    /// <param name="timestamp">The timestamp of when the cached object was generated</param>
    void StoreCall (string service, string method, PyDataType data, long timestamp);

    /// <summary>
    /// Searches for a list of cache hints that hold information about the cached object
    /// </summary>
    /// <param name="list">The list of hints to look for, key is used as cached object name and value is used as name for the client</param>
    /// <returns>A dictionary ready for the EVE Client with the hints for the cached objects</returns>
    PyDictionary <PyString, PyDataType> GetHints (Dictionary <string, string> list);

    /// <summary>
    /// Searches for a specific cached's object hint that holds information about the cached object
    /// </summary>
    /// <param name="name">The cached object to get the hint for</param>
    /// <returns>An object ready for the EVE Client with the hint information for the cached object</returns>
    PyDataType GetHint (string name);

    /// <summary>
    /// Searches for a specific cached object's hint generated by the given <paramref name="service"/> and <paramref name="method"/>
    /// that holds information about the cached object
    /// </summary>
    /// <param name="service">The service that generated the cached object</param>
    /// <param name="method">The method that generated the cached object</param>
    /// <returns>An object ready for the EVE Client with the hing information for the cached object</returns>
    PyDataType GetHint (string service, string method);

    /// <summary>
    /// Runs the given <paramref name="query"/> and stores the result as a cached object identified by <paramref name="name"/>
    /// </summary>
    /// <param name="name">The name of the cached object</param>
    /// <param name="query">The SQL query to run</param>
    /// <param name="type">How the result will be stored inside the cache</param>
    void Load (string name, string query, CacheObjectType type);

    /// <summary>
    /// Runs the given <paramref name="query"/> and stores the result as a cached object identified by the <paramref name="service"/> and
    /// <paramref name="method"/> that generated it
    /// </summary>
    /// <param name="service">The service that generated the cached object</param>
    /// <param name="method">The method that generated the cached object</param>
    /// <param name="query">The SQL query to run</param>
    /// <param name="type">How the result will be stored inside the cache</param>
    void Load (string service, string method, string query, CacheObjectType type);

    /// <summary>
    /// Runs the given <paramref name="queries"/> and stores the result as a cached object identified by the <paramref name="names"/>
    ///
    /// The key of the dictionary is used as name. The values in queries and types should be in the order the names were added
    /// to the dictionary
    /// </summary>
    /// <param name="names">The list of names to store the cached objects as</param>
    /// <param name="queries">The queries to run</param>
    /// <param name="types">How to store each of the cached objects</param>
    /// <exception cref="ArgumentOutOfRangeException">The number of queries, names and types do not match</exception>
    void Load (Dictionary <string, string> names, string [] queries, CacheObjectType [] types);

    /// <summary>
    /// Removes the given cached data from the cache storage
    /// </summary>
    /// <param name="name"></param>
    void Remove (string name);

    /// <summary>
    /// Removes the given cached data for a service call from the cache storage
    /// </summary>
    /// <param name="service"></param>
    /// <param name="method"></param>
    void Remove (string service, string method);
}