﻿using Akavache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using MyIssues.Models;
using MyIssues.DataAccess.Mapping;

namespace MyIssues.DataAccess
{
    public partial class Storage
    {
        public async Task<string> GetToken()
        {
            try
            {
                return await BlobCache.Secure.GetObject<string>(Token);
            }
            catch
            {
                return null;
            }
        }

        public async Task SaveToken(string token)
        {
            await BlobCache.Secure.InsertObject<string>(Token,token);
        }

        public async Task<string> GetCurrentLogin()
        {
            try
            {
                return await BlobCache.LocalMachine.GetObject<string>(Login);
            }
            catch
            {
                return null;
            }
        }

        public async Task SaveCurrentLogin(string login)
        {
            await BlobCache.LocalMachine.InsertObject<string>(Login, login);
        }

        public async Task<long> GetWorkingRepo()
        {
            try
            {
                return await BlobCache.LocalMachine.GetObject<long>(WorkingRepo);
            }
            catch
            {
                return 0;
            }
        }

        public async Task SaveWorkingRepo(long repoId)
        {
            await BlobCache.LocalMachine.InsertObject<long>(WorkingRepo, repoId);
        }
    }
}