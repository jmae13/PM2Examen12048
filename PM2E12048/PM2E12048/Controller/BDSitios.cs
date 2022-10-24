using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PM2E12048.Models;
using SQLite;

namespace PM2E12048.Controller
{
    public class BDSitios
    {

        readonly SQLiteAsyncConnection dbase;

        public BDSitios(string dbpath)
        {
            dbase = new SQLiteAsyncConnection(dbpath);

            /*Se crean las tablas*/
            dbase.CreateTableAsync<Models.Sitios>();

        }

        public Task<int> SitioSave(Sitios sitio)
        {
            if (sitio.id != 0)//update del registro
            {
                return dbase.UpdateAsync(sitio);
            }
            else
            {
                return dbase.InsertAsync(sitio);//inserter nuevo registro
            }

        }

        public Task<List<Sitios>> getListSitio()
        {
            return dbase.Table<Sitios>().ToListAsync();//se convierte el resultado a una lista.
        }

        public async Task<Sitios> getSitio(int pid)
        {
            return await dbase.Table<Sitios>()//se usa explesion lamba
                .Where(i => i.id == pid)
                .FirstOrDefaultAsync();
        }

        public async Task<int> DeleteSitio(Sitios sitio)
        {
            return await dbase.DeleteAsync(sitio);
        }

    }
}
