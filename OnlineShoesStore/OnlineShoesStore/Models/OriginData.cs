﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace OnlineShoesStore.Models
{
    public class OriginDTO
    {
        private int originId;
        private string name;
        private bool isDeleted;

        public OriginDTO(int originId, string name, bool isDeleted)
        {
            this.originId = originId;
            this.name = name;
            this.isDeleted = isDeleted;
        }

        public int OriginId
        {
            get { return originId; }
            set { originId = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public bool IsDeleted
        {
            get { return isDeleted; }
            set { isDeleted = value; }
        }
    }

    public class OriginData
    {
        public List<OriginDTO> GetOrigins()
        {
            List<OriginDTO> result = null;
            string sql = "Select * From Origins Where isDeleted = @Deleted";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@Deleted", false);
            SqlDataReader dr = cmd.ExecuteReader();
            int originId;
            string name;
            result = new List<OriginDTO>();
            while (dr.Read())
            {
                originId = dr.GetInt32(0);
                name = dr.GetString(1);
                result.Add(new OriginDTO(originId, name, false));
            }
            cnn.Close();
            return result;
        }

        public string GetOriginNameByID(int id) {
            string originName = "";
            string sql = "Select name From Origins Where originID = @originID";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@originID", id);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read()) {
                originName = (string)dr[0];
            }
            cnn.Close();
            return originName;
        }
    }
}
