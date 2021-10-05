using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

    public class SaviorComposer : IUserComposer
    {
    public void Compose(Composition composition)
    {
        composition.Components().Append<SaviorComponent>();
    }
    }
    public class SaviorComponent : IComponent
    {
        private IProfilingLogger _logger;
        private readonly IContentTypeService _cts;
        private readonly IDataTypeService _dts;
        private readonly IFileService _fs;

        public SaviorComponent(IProfilingLogger logger, IContentTypeService cts, IDataTypeService dts, IFileService fs)
        {
            _logger = logger;
            _cts = cts;
            _dts = dts;
            _fs = fs;
        }

        public void Initialize()
        {
        #region doctypes
        IEnumerable<string> collection = _cts.GetAllContentTypeAliases();
        string[] aliases = collection.Cast<string>().ToArray();
        IEnumerable<int> doctypeIds = _cts.GetAllContentTypeIds(aliases);
        foreach(int id in doctypeIds)
        {
            IContentType target = _cts.Get(id);
            if (target==null) { }
            else {
            _cts.Save(target, -1);
                _logger.Info<SaviorComponent>("_____Savior saved doctype: {Name}", target.Name);
                Debug.WriteLine("Saved " + target.Name);
            }
        }
        #endregion
        #region datatypes
        IEnumerable<IDataType> datatypes = _dts.GetAll();
        foreach(IDataType datatype in datatypes)
        {
            if (datatype == null) { }
            else { 
            _dts.Save(datatype, -1);
                _logger.Info<SaviorComponent>("_____Savior saved datatype: {Name}", datatype.Name);
                Debug.WriteLine("Saved " + datatype.Name);
            }
        }
        IEnumerable<ITemplate> templates = _fs.GetTemplates();
        #endregion
        #region containers
        /*
        IEnumerable<EntityContainer> containers = _dts.GetContainers()
        {

        }
        */
        #endregion
        #region templates
        /*
        foreach (ITemplate template in templates)
        {
            if (template == null) { }
            else
            {
                _fs.SaveTemplate(template, -1);
                _logger.Info<SaviorComponent>("_____Savior saved template: {Name}", template.Name);
                Debug.WriteLine("Saved " + template.Name);
            }
        }*/
        #endregion
    }
    public void Terminate()
        {
        }
    }