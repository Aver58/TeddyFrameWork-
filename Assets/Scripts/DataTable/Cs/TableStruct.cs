﻿//THIS FILE IS GENERATED BY tabtool, DO NOT EDIT IT!
//GENERATE TIME [2022/9/1 0:21:49]

  public class tbsIdCount : ITableObject
  {
      int id;
      int count;
      public bool FromString(string s)
      {
          DataReader dr = new DataReader();

          var vs = s.Split(',');
          if (vs.Length != 2)
          {
              return false;
          }

		id = dr.GetInt(vs[0]);
		count = dr.GetInt(vs[1]);
          return true;
      }
  };

  public class tbsKeyValue : ITableObject
  {
      int key;
      int value;
      public bool FromString(string s)
      {
          DataReader dr = new DataReader();

          var vs = s.Split(',');
          if (vs.Length != 2)
          {
              return false;
          }

		key = dr.GetInt(vs[0]);
		value = dr.GetInt(vs[1]);
          return true;
      }
  };

  public class tbsTest : ITableObject
  {
      int a;
      string b;
      float c;
      public bool FromString(string s)
      {
          DataReader dr = new DataReader();

          var vs = s.Split(',');
          if (vs.Length != 2)
          {
              return false;
          }

		a = dr.GetInt(vs[0]);
		b = vs[1];
		c = dr.GetFloat(vs[2]);
          return true;
      }
  };

