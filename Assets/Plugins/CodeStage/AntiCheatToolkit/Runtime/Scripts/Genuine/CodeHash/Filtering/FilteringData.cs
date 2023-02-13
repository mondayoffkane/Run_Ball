using System.Collections.Generic;

namespace CodeStage.AntiCheat.Genuine.CodeHash
{
    internal class FilteringData
    {
        public FileFilter[] Includes { get; }
        public FileFilter[] Ignores { get; }
        
        public static FilteringData GetFileFiltersAndroid(bool il2Cpp)
        {
        	var includes = new List<FileFilter>
        	{
        		new FileFilter
        		{
        			filterFileName = "classes",
        			filterExtension = "dex"
        		},
        		new FileFilter
        		{
        			filterFileName = "libunity",
        			filterExtension = "so"
        		},
        		new FileFilter
        		{
        			filterFileName = "libil2cpp",
        			filterExtension = "so"
        		},
        		new FileFilter
        		{
        			filterFileName = "libmain",
        			filterExtension = "so"
        		},
        		new FileFilter
        		{
        			filterFileName = "libMonoPosixHelper",
        			filterExtension = "so"
        		},
        		new FileFilter
        		{
        			filterFileName = "libmonobdwgc",
        			filterExtension = "so"
        		},
        		new FileFilter
        		{
        			filterFileName = "global-metadata",
        			filterExtension = "dat"
        		}
        	};
        
        	if (!il2Cpp)
        	{
	            includes.Add(new FileFilter
        		{
        			filterExtension = "dll"
        		});
        	}
        
        	return new FilteringData(includes.ToArray(), null);
        }
        
        public static FilteringData GetFileFiltersStandaloneWindows(bool il2Cpp)
        {
	        var includes = new List<FileFilter>
	        {
		        new FileFilter
		        {
			        filterExtension = "dll",
		        },
		        new FileFilter
		        {
			        filterExtension = "exe"
		        },
		        
		        // uncomment to hash not only code but also all data
		        // WARNING: this can significantly increase hashing duration!
		        
		        /*new FileFilter
		        {
			        filterPath = "_Data\\",
			        caseSensitive = true,
			        exactFolderMatch = false,
		        },*/
	        };

	        FileFilter[] ignores;

	        if (il2Cpp)
	        {
		        includes.Add(new FileFilter
		        {
			        filterFileName = "global-metadata",
			        filterExtension = "dat",
		        });
		        
		        ignores = new[]
		        {
			        new FileFilter
			        {
				        filterPath = "_BackUpThisFolder_ButDontShipItWithYourGame",
				        caseSensitive = true,
				        exactFolderMatch = false,
			        }
		        };
	        }
	        else
	        {
		        ignores = null;
	        }

	        return new FilteringData(includes.ToArray(), ignores);
        }

        public FilteringData(FileFilter[] includes, FileFilter[] ignores)
        {
            Includes = includes;
            Ignores = ignores;
        }
        
        public bool IsIncluded(string path)
        {
            return IsPathMatchesFilters(path, Includes);
        }

        public bool IsIgnored(string path)
        {
	        if (Ignores == null || Ignores.Length == 0)
		        return false;
	        
            return IsPathMatchesFilters(path, Ignores);
        }

        private bool IsPathMatchesFilters(string path, FileFilter[] filters)
        {
            foreach (var filter in filters)
            {
                if (filter.MatchesPath(path))
                    return true;
            }

            return false;
        }
    }
}