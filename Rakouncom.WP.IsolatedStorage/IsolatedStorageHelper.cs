using System;
using System.IO;
using System.IO.IsolatedStorage;


namespace Rakouncom.WP.IsolatedStorage
{
    public static class IsolatedStorageHelper
    {
        /// <summary>
        /// Créer un répertoire dans l'isolated storage.
        /// </summary>
        /// <param name="directoryName">Le chemin d'accès complet du répertoire à créer.</param>
        public static void CreateDirectory(string directoryName)
        {
            try
            {
                using (IsolatedStorageFile myIsolatedStorage =
                            IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (!string.IsNullOrEmpty(directoryName) &&
                        !myIsolatedStorage.DirectoryExists(directoryName))
                    {
                        myIsolatedStorage.CreateDirectory(directoryName);
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO:
            }
        }

        /// <summary>
        /// Supprime un répertoire de l'isolated storage.
        /// </summary>
        /// <param name="directoryName">Le chemin complet du répertoire à supprimer.</param>
        public static void DeleteDirectory(string directoryName)
        {
            try
            {
                using (IsolatedStorageFile myIsolatedStorage =
                            IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (!string.IsNullOrEmpty(directoryName) &&
                        !myIsolatedStorage.DirectoryExists(directoryName))
                    {
                        myIsolatedStorage.DeleteDirectory(directoryName);
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO:
            }
        }

        /// <summary>
        /// Renome un répertoire de l'isoltage storage.
        /// </summary>
        /// <remarks>
        /// Penser à préciser le chemin d'accès complet des répertoires.
        /// </remarks>
        /// <param name="sourceDirectoryName">Le chemin d'accès complet du répertoire à renommer.</param>
        /// <param name="targetDirectoryName">Le chemin d'accès complet du répertoire renommé.</param>
        public static void RenameDirectory(string sourceDirectoryName, string targetDirectoryName)
        {
            try
            {
                using (IsolatedStorageFile myIsolatedStorage = 
                            IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (!string.IsNullOrEmpty(sourceDirectoryName) &&
                        !string.IsNullOrEmpty(targetDirectoryName) &&
                        myIsolatedStorage.DirectoryExists(sourceDirectoryName) &&
                        !myIsolatedStorage.DirectoryExists(targetDirectoryName))
                    {
                        myIsolatedStorage.MoveDirectory(
                                    sourceDirectoryName,
                                    targetDirectoryName);
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO:
            }
        }

        /// <summary>
        /// Sauvegarde un fichier binaire dans l'isolated storage.
        /// </summary>
        /// <remarks>
        /// Penser à fermer le flux dans la méthode appelante.
        /// </remarks>
        /// <param name="fileName">Le chemin d'accès complet du fichier à créer.</param>
        /// <param name="theStream">Le flux à copier dans le nouveau fichier.</param>
        public static void SaveBinaryFile(string fileName, Stream theStream)
        {
            try
            {
                using (IsolatedStorageFile myIsolatedStorage =
                    IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (myIsolatedStorage.FileExists(fileName))
                    {
                        myIsolatedStorage.DeleteFile(fileName);
                    }
                    using (IsolatedStorageFileStream targetStream =
                                myIsolatedStorage.OpenFile(
                                        fileName, FileMode.Create,
                                        FileAccess.Write))
                    {
                        // Initialize the buffer for 4KB disk pages.
                        byte[] readBuffer = new byte[4096];
                        int bytesRead = -1;
                        // Copy the image to isolated storage.
                        while ((bytesRead = theStream.Read(readBuffer, 0, readBuffer.Length)) > 0)
                        {
                            targetStream.Write(readBuffer, 0, bytesRead);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                //TODO:
            }
        }

        /// <summary>
        /// Lit un fichier binaire.
        /// </summary>
        /// <remarks>
        /// Penser à fermer le flux dans la méthode appelante.
        /// </remarks>
        /// <param name="fileName">Le chemin d'accès complet au fichier binaire.</param>
        /// <returns>Le fichier binaire.</returns>
        public static IsolatedStorageFileStream ReadBinaryFile(string fileName)
        {
            IsolatedStorageFileStream result = null;
            try
            {
                using (IsolatedStorageFile myIsolatedStorage =
                    IsolatedStorageFile.GetUserStoreForApplication())
                {
                    IsolatedStorageFileStream fileStream =
                        myIsolatedStorage.OpenFile(fileName, FileMode.Open, FileAccess.Read);

                        result = fileStream;
                }
            }
            catch (Exception)
            {
                //TODO:
            }

            return result;
        }

        /// <summary>
        /// Créer un fichier binaire dans l'isolated storage.
        /// </summary>
        /// <param name="filePath">Le chemin d'accès complet au fichier.</param>
        /// <returns>Le fichier binaire.</returns>
        public static IsolatedStorageFileStream CreateFile(string filePath)
        {
            IsolatedStorageFileStream result = null;
            try
            {
                using (IsolatedStorageFile myIsolatedStorage =
                            IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (myIsolatedStorage.FileExists(filePath))
                    {
                        myIsolatedStorage.DeleteFile(filePath);
                    }

                    result = myIsolatedStorage.CreateFile(filePath);
                }
            }
            catch (Exception)
            {
                
                throw;
            }

            return result;
        }
    }
}
