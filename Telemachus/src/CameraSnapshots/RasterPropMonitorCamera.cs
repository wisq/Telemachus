﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telemachus.CameraSnapshots
{
    class RasterPropMonitorCamera : PartModule
    {
        protected PartModule rpmPartModule;
        public PartModule rpmCameraModule
        {
            get
            {
                if (rpmPartModule == null)
                {
                    foreach (PartModule module in part.Modules)
                    {
                        if (module.moduleName == "JSIExternalCameraSelector")
                        {
                            //PluginLogger.debug("GOT MODULE");
                            rpmPartModule = module;
                        }
                    }
                }

                return rpmPartModule;
            }
        }

        protected string rpmCameraName;
        public string cameraName
        {
            get
            {
                if(rpmCameraName == null)
                {
                    if (rpmCameraModule != null)
                    {
                        rpmCameraName = (String)getRPMField("cameraIDPrefix") + (int)getRPMField("current");
                    }
                }

                return rpmCameraName;
            }
        }

        protected UnityEngine.Vector3 rpmRotateCamera;
        public UnityEngine.Vector3 rotateCamera
        {
            get
            {
                if (rpmRotateCamera == UnityEngine.Vector3.zero)
                {
                    if (rpmCameraModule != null)
                    {
                        rpmRotateCamera = (UnityEngine.Vector3)getRPMField("rotateCamera");
                    }
                }

                return rpmRotateCamera;
            }
        }

        public UnityEngine.Vector3 cameraRotation()
        {
            return (UnityEngine.Vector3)getRPMField("rotateCamera");
        }

        protected UnityEngine.Vector3 rpmTranslateCamera;
        public UnityEngine.Vector3 translateCamera
        {
            get
            {
                if (rpmTranslateCamera == UnityEngine.Vector3.zero)
                {
                    if (rpmCameraModule != null)
                    {
                        rpmTranslateCamera = (UnityEngine.Vector3)getRPMField("translateCamera");
                    }
                }

                return rpmTranslateCamera;
            }
        }

        public override void OnStart(PartModule.StartState state)
        {
            DebugInfo();
            CameraCaptureManager.Instance.BroadcastMessage("addCamera", this);
        }

        public void OnDestroy()
        {
            CameraCaptureManager.Instance.BroadcastMessage("removeCamera", this.cameraName);
        }

        public void Update()
        {
            //DebugInfo();
        }

        // DEBUGGING

        public object getRPMField(string name)
        {
            if (rpmCameraModule == null)
            {
                return null;
            }

            return rpmCameraModule.Fields.GetValue(name);
        }

        public string fieldValues()
        {
            string val = "";
            foreach (BaseField field in Fields)
            {
                val += field.name + " : " + field.originalValue + " || ";
            }

            return val;
        }

        public string partModules()
        {
            string val = "";
            foreach (PartModule module in part.Modules)
            {
                val += module.moduleName + " ||";
            }

            return val;
        }

        public string debugRPMFields()
        {
            string val = "";
            PartModule rpmModule = null;

            foreach (PartModule module in part.Modules)
            {
                if (module.moduleName == "JSIExternalCameraSelector")
                {
                    rpmModule = module;
                }
            }

            if (rpmCameraModule == null)
            {
                return "NA";
            }

            foreach (BaseField field in rpmCameraModule.Fields)
            {
                val += field.name + " : " + field.originalValue + " || ";
            }

            return val;
        }

        public void DebugInfo()
        {
            PluginLogger.debug("RPM CAMERA LOADED: " + part.name + " ; NAME: " + cameraName + " ; POS: " + part.transform.position + "; ROTATION: " + rotateCamera + " ; TRANSLATE: " + translateCamera);
        }
    }
}
