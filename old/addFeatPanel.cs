using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Windows;


namespace AlphaCodex
{
    public partial class MainWindow : Window
    {
        //#region feat initialization

        ///** properties **/
        //private ObservableCollection<string> _typesList = new ObservableCollection<string>();
        //public ObservableCollection<string> typesList
        //{
        //    get { return _typesList; }
        //    set
        //    {
        //        _typesList = value;
        //        NotifyPropertyChanged("typesList");
        //    }
        //}
        ///** Methods **/

        //private void AddFeatsPanel_Initialized(object sender, System.EventArgs e)
        //{
        //    getTypesListAdd();
        //    RegisterSpell();
        //}

        ///* add new feat to the database */
        //private void AddFeatSave_Click(object sender, RoutedEventArgs e)
        //{
        //    //string name = addFeatName.Text;
        //    //string description = addFeatDescription.Text;
        //    //string type = addFeatType.SelectedIndex.ToString();
        //    //string prerequisite = addFeatPrerequisite.Text;
        //    //string benefit = addFeatBenefit.Text;
        //    //string normal = addFeatNormal.Text;
        //    //string special = addFeatSpecial.Text;
        //    //string fighter = "0";
        //    //if (addFeatFighter.IsChecked == true)
        //    //    fighter = "1";
        //    //string page = addFeatPage.Text;
        //    //string book = addFeatBook.SelectedIndex.ToString();
        //    //string info = addFeatInfo.Text;

        //    //string queryString = "INSERT INTO feat ( name, description, type, prerequisite, benefit, normal, special, fighter, book_page, book_id, info) VALUES (@name, @description, @type, @prerequisite, @benefit, @normal, @special, @fighter, @page, @book, @info)";

        //    //using (SQLiteConnection connDB = new SQLiteConnection(MainWindow.conn))
        //    //{
        //    //    SQLiteCommand command = connDB.CreateCommand();
        //    //    command.CommandText = queryString;
        //    //    command.Parameters.Add(new SQLiteParameter("@name", name));
        //    //    command.Parameters.Add(new SQLiteParameter("@description", description));
        //    //    command.Parameters.Add(new SQLiteParameter("@type", type));
        //    //    command.Parameters.Add(new SQLiteParameter("@prerequisite", prerequisite));
        //    //    command.Parameters.Add(new SQLiteParameter("@benefit", benefit));
        //    //    command.Parameters.Add(new SQLiteParameter("@normal", normal));
        //    //    command.Parameters.Add(new SQLiteParameter("@special", special));
        //    //    command.Parameters.Add(new SQLiteParameter("@fighter", fighter));
        //    //    command.Parameters.Add(new SQLiteParameter("@page", page));
        //    //    command.Parameters.Add(new SQLiteParameter("@book", book));
        //    //    command.Parameters.Add(new SQLiteParameter("@info", info));

        //    //    command.ExecuteNonQuery();
        //    //}
        //}

        ///* Retrieve the list of type a feat can have from the DB*/
        //private void getTypesListAdd()
        //{
        //    string queryString = "SELECT name FROM 'feat_type'";

        //    using (SQLiteConnection connDB = new SQLiteConnection(MainWindow.conn))
        //    {
        //        connDB.Open();
        //        SQLiteCommand command = new SQLiteCommand(queryString, connDB);
        //        SQLiteDataReader reader = command.ExecuteReader();
        //        typesList.Clear();
        //        typesList.Add("");
        //        while (reader.Read())
        //        {
        //            typesList.Add(reader["name"].ToString());
        //        }
        //        reader.Close();
        //        connDB.Close();
        //    }
        //}
        //#endregion
    }
}