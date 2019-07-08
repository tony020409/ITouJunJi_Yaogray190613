using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace GameSysc
{
    public class GameSyscObjectPool
    {
        public List<IPlayerMono> _aGameFrameObjects = new List<IPlayerMono>();
        List<IPlayerMono> _aFinished = new List<IPlayerMono>();

        private object _Locker = new object();

        public void f_Reg(IPlayerMono tIPlayerMono)
        {
            lock (_Locker)
            {
                IPlayerMono tFindIPlayerMono = _aGameFrameObjects.Find(delegate (IPlayerMono tItem)
                                                                    {
                                                                        if (tItem == tIPlayerMono)
                                                                        {
                                                                            return true;
                                                                        }
                                                                        return false;
                                                                    }
                    );
                if (tFindIPlayerMono == null)
                {
                    _aGameFrameObjects.Add(tIPlayerMono);
                }
            }
        }

        void UnReg(IPlayerMono tIPlayerMono)
        {
           _aGameFrameObjects.Remove(tIPlayerMono);
        }

        public void f_Update(int iGameFramesPerSecond)
        {
            lock (_Locker)
            {
                for (int i = _aGameFrameObjects.Count - 1; i >= 0; --i)
                {
                    _aGameFrameObjects[i].f_Update(iGameFramesPerSecond);
                    if (_aGameFrameObjects[i].m_bIsComplete)
                    {
                        _aFinished.Add(_aGameFrameObjects[i]);
                    }
                }
                if (_aFinished.Count > 0)
                {
                    //foreach (IPlayerMono obj in _aFinished)
                    for (int i = 0; i < _aFinished.Count; i++)
                    {
                        UnReg(_aFinished[i]);
                    }
                    _aFinished.Clear();
                }
            }
        }

        public void f_Reset()
        {
            _aGameFrameObjects.Clear();
            _aFinished.Clear();
        }

    }



}