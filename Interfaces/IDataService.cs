﻿namespace DemyAI.Interfaces;

public interface IDataService<T> {

    void ListenForChanges<T>(string nodeName, string uid, Action<T> onDataChanged);

    Task<ObservableCollection<T>> GetAllAsync<T>(string nodeName);

    Task<string> AddAsync<T>(string nodeName, T newItem, string? customID = null);

    Task<T?> GetByEmailAsync(string nodeName, string email);

    Task<T> GetByUidAsync<T>(string nodeName, string Uid);

    Task<List<DemyUser>> GetByRole(string role);

    Task UpdateAsync<T>(string nodeName, string uid, string propertyValue, string propertyName);

    Task UpdateAsync<T>(string nodeName, string uid, T newData);

    Task DeleteAsync(string NodeName, string uid);
}
