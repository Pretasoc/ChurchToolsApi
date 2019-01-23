using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChurchToolsApi
{
    public abstract class ChurchtoolsModule
    {
        private readonly ChurchToolsSession _session;

        protected ChurchtoolsModule(ChurchToolsSession session)
        {
            _session = session;
        }
        protected abstract string Path { get; }

        public Task<string> GetModuleNameAsync()
        {
            return InvokeApiRequestAsync<string>(null, "getModuleNameAsync");
        }

        public Task<string> GetModulePath()
        {
            return InvokeApiRequestAsync<string>(null, "getModulePathAsync");
        }

        public Task<string> GetMasterData()
        {
            return InvokeApiRequestAsync<string>(null, "getMasterDataAsync");
        }

        public Task SaveMasterData(int id, string tableName, IReadOnlyDictionary<string, object> values)
        {
            return SaveMasterData(id, tableName, values, CancellationToken.None);
        }

        public Task SaveMasterData(int id, string tableName, IReadOnlyDictionary<string, object> values, CancellationToken cancellationToken)
        {
            return InvokeApiRequestAsync(new MasterDataRow(), cancellationToken, "saveMasterData");
        }

        public Task DeleteMasterData(IEnumerable<(int Id, string table)> rows)
        {
            return DeleteMasterData(CancellationToken.None, rows.ToArray());
        }

        public Task DeleteMasterData(IEnumerable<(int Id, string table)> rows, CancellationToken cancellationToken)
        {
            return DeleteMasterData(cancellationToken, rows.ToArray());
        }

        public Task DeleteMasterData(params (int Id, string table)[] rows)
        {
            return DeleteMasterData(CancellationToken.None, rows);
        }

        public Task DeleteMasterData(CancellationToken cancellationToken, params (int Id, string table)[] rows)
        {
            return InvokeApiRequestAsync(rows, "deleteMasterData");
        }



        protected Task<T> InvokeApiRequestAsync<T>(object parameter, [CallerMemberName] string functionName = null)
        {
            return InvokeApiRequestAsync<T>(parameter, CancellationToken.None, functionName);
        }

        protected Task<T> InvokeApiRequestAsync<T>(object parameter, CancellationToken cancellationToken, [CallerMemberName] string functionName = null)
        {
            return _session.InvokeApiRequestAsync<T>(Path, functionName, parameter, cancellationToken);
        }

        protected Task<(bool succeeded, T value)> TryInvokeApiRequestAsync<T>(object parameter, [CallerMemberName] string functionName = null)
        {
            return TryInvokeApiRequestAsync<T>(parameter, CancellationToken.None, functionName);
        }

        protected async Task<(bool succeeded, T value)> TryInvokeApiRequestAsync<T>(object parameter, CancellationToken cancellationToken, [CallerMemberName] string functionName = null)
        {
            try
            {
                var result = await _session.InvokeApiRequestAsync<T>(Path, functionName, parameter, cancellationToken);
                return (true, result);
            }
            catch(RequestFailedException)
            { 
                return (false, default);
            }
        }

        protected Task InvokeApiRequestAsync(object parameter, [CallerMemberName] string functionName = null)
        {
            return InvokeApiRequestAsync(parameter, CancellationToken.None, functionName);
        }

        protected Task InvokeApiRequestAsync(object parameter, CancellationToken cancellationToken, [CallerMemberName] string functionName = null)
        {
            return _session.InvokeApiRequestAsync(Path, functionName, parameter, cancellationToken);
        }

        protected Task<bool> TryInvokeApiRequestAsync(object parameter, [CallerMemberName] string functionName = null)
        {
            return TryInvokeApiRequestAsync(parameter, CancellationToken.None, functionName);
        }

        protected async Task<bool> TryInvokeApiRequestAsync(object parameter, CancellationToken cancellationToken, [CallerMemberName] string functionName = null)
        {
            try
            {
                await _session.InvokeApiRequestAsync(Path, functionName, parameter, cancellationToken);
            }
            catch(RequestFailedException)
            {
                return false;
            }
            return true;
        }

        public Task SaveSettingAsync(string setting, object value)
        {
            return SaveSettingAsync(setting, value, CancellationToken.None);
        }

        public Task SaveSettingAsync(string setting, object value, CancellationToken cancellationToken)
        {
            return InvokeApiRequestAsync(new SettingsParameter() { Key = setting, Value = value}, cancellationToken, "saveSetting");   
        }

        public Task<IReadOnlyDictionary<string, dynamic>> GetSettingsAsync()
        {
            return GetSettingsAsync(CancellationToken.None);
        }

        public async Task<IReadOnlyDictionary<string, dynamic>> GetSettingsAsync(CancellationToken cancellationToken)
        {
            return await InvokeApiRequestAsync<Dictionary<string, dynamic>>(null,  cancellationToken,"getSettings");
        }

        public Task<IReadOnlyList<ChurchTable>> GetMasterdataTablenames()
        {
            return GetMasterdataTablenames(CancellationToken.None);
        }

        public async Task<IReadOnlyList<ChurchTable>> GetMasterdataTablenames(CancellationToken cancellationToken)
        {
            return await InvokeApiRequestAsync<List<ChurchTable>>(null, cancellationToken, "getMasterDataTablenames");
        }

    }

    public interface IMasterTableRow
    {
    }
}
