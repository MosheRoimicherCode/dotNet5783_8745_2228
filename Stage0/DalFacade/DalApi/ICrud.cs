﻿using DO;

public interface ICrud<T>
{
    void Add(T t);
    void Delete(int n);
    void Update(int n, T t);
    T Get(int n);
    void GetAll();
    List<T> CopyList();
}

