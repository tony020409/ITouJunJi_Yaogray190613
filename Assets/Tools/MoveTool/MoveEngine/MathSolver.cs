using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MathClasses {

	public delegate Vector3 DelGetPoint(float t);

	public class Bezier {
		Vector3[] points = new Vector3[6];
		Vector3 a, b, c, d, e, f, g, h, i;
		float t2, t3, t4;

		//public delegate float DelGetVelocity(float t);
		public DelGetPoint GetPoint, GetVelocity;
		//public DelGetVelocity GetVelocity;

		public void AssignPoints(List<Vector3> p) {
			int n = p.Count;
			for (int i = 0; i < n; i++) {
				points[i] = p[i];
			}
			#region ============== Assign Bezier2 Coefficients ================
			if (n == 3) {
				a = points[0] - 2 * points[1] + points[2];
				b = 2 * (-points[0] + points[1]);
				c = 2 * a;
				GetPoint = GetPoint2;
				GetVelocity = GetVelocity2;
				#endregion ========================================================
				#region ============== Assign Bezier3 Coefficients =================
			} else if (n == 4) {                                                   // Cubic Bezier Curve
				a = -points[0] + 3 * (points[1] - points[2]) + points[3];
				b = 3 * (points[0] - 2 * points[1] + points[2]);
				c = 3 * (-points[0] + points[1]);
				d = 3 * a;
				e = 2 * b;
				GetPoint = GetPoint3;
				GetVelocity = GetVelocity3;
				#endregion ========================================================
				#region ============== Assign Bezier4 Coefficients =================
			} else if (n == 5) {                                            // 4th Bezier Curve
				a = points[0] - 4 * points[1] + 6 * points[2] - 4 * points[3] + points[4];
				b = -4 * points[0] + 12 * points[1] - 12 * points[2] + 4 * points[3];
				c = 6 * points[0] - 12 * points[1] + 6 * points[2];
				d = -4 * points[0] + 4 * points[1];
				e = 4 * a;
				f = 3 * b;
				g = 2 * c;
				GetPoint = GetPoint4;
				GetVelocity = GetVelocity4;
				#endregion ========================================================
				#region ============== Assign Bezier5 Coefficients =================
			} else if (n == 6) {                                            // 5th Bezier Curve
				a = -points[0] + 5 * points[1] - 10 * points[2] + 10 * points[3] - 5 * points[4] + points[5];
				b = 5 * points[0] - 20 * points[1] + 30 * points[2] - 20 * points[3] + 5 * points[4];
				c = -10 * points[0] + 30 * points[1] - 30 * points[2] + 10 * points[3];
				d = 10 * points[0] - 20 * points[1] + 10 * points[2];
				e = -5 * points[0] + 5 * points[1];
				f = 5 * a;
				g = 4 * b;
				h = 3 * c;
				i = 2 * d;
				GetPoint = GetPoint5;
				GetVelocity = GetVelocity5;
				#endregion ========================================================
			} else {
				GetPoint = null;
				GetVelocity = null;
			}
		}
		/// <summary>
		/// 取得曲線長度，從 0 積分到 max (數值積分，積分精度為: 0.02)
		/// </summary>
		public float GetLength(float max = 1) {
			float step = 0.02f;
			float len = 0;
			for (float t = 0; t <= max; t += step) {
				len += GetVelocity(t).magnitude * step;
			}
			return len;
		}

		Vector3 GetPoint2(float t) {
			return (t * t * a) + (t * b) + points[0];
		}

		Vector3 GetVelocity2(float t) {
			return ((t * c) + b);
		}

		Vector3 GetPoint3(float t) {
			t2 = t * t;
			return (t2 * t * a) + (t2 * b) + t * c + points[0];
		}

		Vector3 GetVelocity3(float t) {
			return ((t * t * d) + (t * e) + c);
		}

		Vector3 GetPoint4(float t) {
			t2 = t * t;
			t3 = t * t2;
			return t3 * t * a + t3 * b + t2 * c + t * d + points[0];
		}

		Vector3 GetVelocity4(float t) {
			t2 = t * t;
			return (t2 * t * e + t2 * f + t * g + d);
		}

		Vector3 GetPoint5(float t) {
			t2 = t * t;
			t3 = t * t2;
			t4 = t * t3;
			return t4 * t * a + t4 * b + t3 * c + t2 * d + t * e + points[0];
		}

		Vector3 GetVelocity5(float t) {
			t2 = t * t;
			t3 = t * t2;
			return (t3 * t * f + t3 * g + t2 * h + t * i + e);
		}
	}

	public class Line {
		Vector3 from, dir;
		public float mag;

		public void SetLine(Vector3 from, Vector3 to) {
			this.from = from;
			dir = to - from;
			mag = dir.magnitude;
		}

		public Vector3 GetPoint(float t) {
			return from + t * dir;
		}

		public Vector3 GetVelocity(float t) {
			return dir;
		}
	}

	public class QuadraticCurve {
		Vector3 c0, c1, c2;
		Vector3 c02;

		/// <summary>
		/// 定義一條拋物線 (t=0 時在初始位置, t=1 時在終點位置)
		/// </summary>
		/// <param name="start">初始位置</param>
		/// <param name="to">終點位置</param>
		/// <param name="startDir">初始位置方向</param>
		/// <param name="endDir">終點位置方向</param>
		public void SetCurve(Vector3 start, Vector3 to, Vector3 startDir = default(Vector3), Vector3 endDir = default(Vector3)) {
			if (startDir != default(Vector3)) {             // 指定初始方向 (起點跟終點都指定也是用這個，因為二者只能選其一)
				c0 = -start + to - startDir;
				c1 = startDir;
				c2 = start;
			} else if (endDir != default(Vector3)) {        // 指定終點方向
				SetCurveWithEndDir(start, to, endDir);
			} else {                                        // 起、終都沒有指定，則終點方向為 終點-起點 的平面投影向量
				SetCurveWithEndDir(start, to, MathSolver.V2Vector(to - start));
			}

			c02 = c0 * 2;
		}

		/// <summary>
		/// 定義一條二次曲線
		/// </summary>
		/// <param name="startPoint">起始點</param>
		/// <param name="startDir">起始方向 (向量長決定力道大小)</param>
		/// <param name="gravity">地心引力大小 (決定下降速度)</param>
		public void SetCurve(Vector3 startPoint, Vector3 startDir, float gravity) {
			c2 = startPoint;
			c1 = startDir;
			c0 = new Vector3(0, -gravity * 0.5f, 0);

			c02 = c0 * 2;
		}

		/// <summary>
		/// 只更改初始方向
		/// </summary>
		/// <param name="startDir"></param>
		public void ChangeDir(Vector3 startDir) {
			c1 = startDir;
		}

		void SetCurveWithEndDir(Vector3 start, Vector3 to, Vector3 endDir) {
			c0 = start - to + endDir;
			c1 = 2 * (-start + to) - endDir;
			c2 = start;
		}

		public Vector3 GetPoint(float t) {
			return (c0 * t * t) + (c1 * t) + c2;
		}

		public Vector3 GetVelocity(float t) {
			return (c02 * t) + c1;
		}

		/// <summary>
		/// 取得曲線長度，從 0 積分到 max (數值積分，積分精度為: 0.02)
		/// </summary>
		public float GetLength(float max = 1) {
			float step = 0.02f;
			float len = 0;
			for (float t = 0; t <= max; t += step) {
				len += GetVelocity(t).magnitude * step;
			}
			return len;
		}

		public QuadraticCurve GetNewInstance() {
			QuadraticCurve proj = new QuadraticCurve();
			proj.SetCurve(c2, c1, -c0.y * 2);
			return proj;
		}
	}
	
	public class Probability {

		int[] pro;
		int r, n, sum;

		/// <summary>
		/// 給一整數陣列，設定機率，陣列大小決定有幾種狀態，設定方式如下:
		/// --- int[] p = new int[] { 20, 30, 50 };
		/// --- SetProb(p); 
		/// --- // 陣列有3個元索，所以會有三種狀態，呼叫 Get() 時，有 20% 機率會得到 0, 30% 機率會得到1, 50% 機率會得到2
		/// </summary>
		/// <param name="probabilities"></param>
		public void SetProb(int[] probabilities) {
			pro = probabilities;
		}

		public int Get() {
			if (pro == null || pro.Length == 0) return -1;
			n = pro.Length;
			if (n == 1) return 0;

			r = Random.Range(0, 100);

			n--;
			sum = 0;
			for (int i = 0; i < n; i++) {
				if (r >= sum && r < sum + pro[i]) {
					return i;
				}
				sum += pro[i];
			}

			return n;
		}
	}

	[System.Serializable]
	public class ArrayManager<T> {

		HashSet<T> hashPool;
		HashSet<T> useable;                       // 空閒的元索
		public bool notFull = false;

		public T[] Useable {
			get {
				T[] array = new T[useable.Count];
				useable.CopyTo(array);
				return array;
			}
		}

		public void SetArray(List<T> array) {
			hashPool = new HashSet<T>(array);
			ReSet();
		}

		public void ReSet() {
			if (hashPool == null) return;
			useable = new HashSet<T>(hashPool);
			notFull = (useable.Count > 0);
		}

		public void Remove(T value) {
			if (hashPool == null || useable == null) return;

			hashPool.Remove(value);
			useable.Remove(value);
			notFull = (useable.Count > 0);
		}
		/// <summary>
		/// 佔用一個位置 (如果傳入的值不在可用的空間裡，回傳 false
		/// </summary>
		public bool TakeOne(T value) {
			if (useable == null) return false;

			if (useable.Contains(value)) {
				useable.Remove(value);
				notFull = (useable.Count > 0);
				return true;
			}
			return false;
		}

		public int UseableCount {
			get { return useable.Count; }
		}

		/// <summary>
		/// 隨機給一個可用的點，在呼叫此函數前需自行在前面用 notFull 變數判斷是否已滿，若滿了返回的值是錯的
		/// </summary>
		/// <returns></returns>
		public T GetRandom() {
			if (useable != null && useable.Count > 0) {
				int randomN = Random.Range(0, useable.Count);

				int count = 0;
				foreach (T value in useable) {
					if (randomN == count) {
						T selected = value;
						useable.Remove(value);
						if (useable.Count == 0) notFull = false;
						return selected;
					}
					count++;
				}
			}
			return default(T);
		}

		public void Release(T value) {
			if (hashPool == null || useable == null) return;

			if (hashPool.Contains(value)) {
				useable.Add(value);                                   // 將被釋放的位置加回空閒陣列
				notFull = true;
			}
		}
	}

	public class MaxAngleVector {

		float angle, cos;

		public MaxAngleVector(float angle = 30, bool isRadian = true) {
			SetAngle(angle, isRadian);
		}

		public void SetAngle(float angle, bool isRadian = true) {
			this.angle = angle;
			if (!isRadian) this.angle *= 0.01745329f;
			cos = Mathf.Cos(this.angle);
		}

		/// <summary>
		/// 若 targetVector 和 centerVector 在角度範圍內，直接回傳該向量，
		/// 否則回傳在 angle 角度的圓錐上，該向量的投影 (回傳的向量已單位化)
		/// </summary>
		public Vector3 GetVector(Vector3 centerVector, Vector3 targetVector) {
			float len1 = centerVector.magnitude;
			float len2 = targetVector.magnitude;
			if (Vector3.Dot(centerVector, targetVector) / len1 / len2 > cos) return targetVector / len2;

			Vector3 orth = targetVector - MathSolver.ProjectOnVector(centerVector, targetVector);
			return centerVector / len1 * Mathf.Cos(angle) + orth.normalized * Mathf.Sin(angle);
		}

	}

}

public class MathSolver {

	public static bool CheckInBetween(Vector3 base1, Vector3 base2, Vector3 v) {
		float q = base1.z * base2.x - base1.x * base2.z;

		float a = -base2.z / q;
		float b = base1.z / q;
		float c = base2.x / q;
		float d = -base1.x / q;

		if ((a * v.x + c * v.z) < 0) return false;
		if ((b * v.x + d * v.z) < 0) return false;
		return true;
	}

	// 給較接近目標那邊的切點
	public static Vector3 CalcTanPoint2D(Vector3 lineOrigin, Vector3 circleCenter, float radius, Vector3 tarPos) {
		float u = lineOrigin.x - circleCenter.x;   // (xp - a)
		float v = lineOrigin.z - circleCenter.z;   // (yp - b)
		float r2 = radius * radius;
		float s = u * u + v * v;

		// 在圓內
		if (s < r2) return new Vector3(float.NaN, 0, 0);

		float q = Mathf.Pow(s - r2, 0.5f);

		// 找到兩個切點
		Vector3[] tanPoints = new Vector3[] {
			new Vector3((r2 * u + radius * v * q) / s + circleCenter.x,
			lineOrigin.y, (r2 * v - radius * u * q) / s + circleCenter.z),
			new Vector3((r2 * u - radius * v * q) / s + circleCenter.x,
			lineOrigin.y, (r2 * v + radius * u * q) / s + circleCenter.z)
		};

		// 找到距離較近的那個點
		// ------ 第一個點的距離 -------
		u = tarPos.x - tanPoints[0].x;
		v = tarPos.z - tanPoints[0].z;
		s = u * u + v * v;
		// ------ 第二個點的距離 -------
		u = tarPos.x - tanPoints[1].x;
		v = tarPos.z - tanPoints[1].z;
		if ((u * u + v * v) > s) {
			return tanPoints[0];
		} else {
			return tanPoints[1];
		}
	}
	/// <summary>
	/// 將向量的 y 值轉為 0
	/// </summary>
	public static Vector3 V2Vector(Vector3 v) {
		return new Vector3(v.x, 0, v.y);
	}
	/// <summary>
	/// 計算 Dot Product (忽略兩向量的 y 值)
	/// </summary>
	public static float V2Dot(Vector3 v1, Vector3 v2) {
		return v1.x * v2.x + v1.z * v2.z;
	}

	public static float V2Magnitude(Vector3 v) {
		return Mathf.Sqrt(v.x * v.x + v.z * v.z);
	}
	/// <summary>
	/// 回傳兩向量間夾角的 Cos 值，若傳入的兩向量都已單位化，可將 isNormalized 設為 True，計算量會較小
	/// </summary>
	public static float V2Cos(Vector3 v1, Vector3 v2, bool isNormalized = false) {
		if (isNormalized) {
			return v1.x * v2.x + v1.z * v2.z;
		} else {
			float d1 = Mathf.Sqrt(v1.x * v1.x + v1.z * v1.z);
			float d2 = Mathf.Sqrt(v2.x * v2.x + v2.z * v2.z);
			return (v1.x * v2.x + v1.z * v2.z) / d1 / d2;
		}
	}

	public static Vector3 RayIntersPlane(Vector3 rayOrigin, Vector3 rayDir, Vector3 planeOrigin, Vector3 planeNormal) {
		float t = Vector3.Dot((planeOrigin - rayOrigin), planeNormal) / Vector3.Dot(rayDir, planeNormal);
		return rayOrigin + rayDir * t;
	}

	public static float V2CosOfVectors(Vector3 v1, Vector3 v2) {
		v1.y = 0;
		v2.y = 0;
		v1.Normalize();
		v2.Normalize();
		return v1.x * v2.x + v1.z * v2.z;
	}
	/// <summary>
	/// 傳入一個陣列，打亂所有元素後回傳
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="array"></param>
	/// <returns></returns>
	public static T[] GetRandomArray<T>(T[] array) {
		T tmp;

		int halfN = array.Length / 2;
		int r;

		for (int i = 0; i < halfN; i++) {
			// 跟隨機一個互換位置
			r = Random.Range(0, array.Length);
			tmp = array[i];
			array[i] = array[r];
			array[r] = tmp;
		}

		return array;
	}

	/// <summary>
	/// 回傳 在 from 向量上，to 向量的投影
	/// </summary>
	public static Vector3 ProjectOnVector(Vector3 from, Vector3 to) {
		float dot = from.x * to.x + from.y * to.y + from.z * to.z;
		return from * dot / from.magnitude;
	}

	/// <summary>
	/// 回傳一個扇形區域內的可站位置
	/// </summary>
	/// <param name="origin">扇形區域的圓心</param>
	/// <param name="fromAng">定義扇形區域的角度 (From)</param>
	/// <param name="toAngle">定義扇形區域的角度 (To)</param>
	/// <param name="nearDist">最近的可站位置</param>
	/// <param name="pointSize">每一個可站位置的大小 (半徑)</param>
	/// <returns></returns>
	public static List<Vector3> CalcSectorPoints(Vector3 origin, float fromAng, float toAngle, float nearDist, float pointSize) {
		float circleR = nearDist + pointSize;                           // 大圓半徑
		float dAng = 2 * Mathf.Asin(pointSize / circleR);                 // 產生的每一個圓在大圓上佔用的角度
		toAngle -= dAng / 2;

		List<Vector3> posPool = new List<Vector3>();

		for (float a = fromAng + dAng / 2; a <= toAngle; a += dAng) {
			posPool.Add(new Vector3(Mathf.Cos(a), 0, Mathf.Sin(a)) * circleR + origin);
		}
		return posPool;
	}

	/// <summary>
	/// 從 posArray 陣列中找到離 pos 最近的點，回傳最近點在 posArray 的索引值，若 posArray 是 null 或數量為 0 回傳 -1
	/// (非最佳演算法，只簡單採用暴力搜尋)
	/// </summary>
	public static int FindClosestPoint(Vector3 pos, Vector3[] posArray) {
		if (posArray == null || posArray.Length == 0) return -1;
		int chosen = 0;
		float dist = float.MaxValue;
		float d;
		for (int i = 0; i < posArray.Length; i++) {
			d = (posArray[i] - pos).sqrMagnitude;
			if (d < dist) {
				dist = d;
				chosen = i;
			}
		}
		return chosen;
	}
	/// <summary>
	/// 輸入 Cosin 值，回傳半角的 Cosin 值 (回傳值恆為正)
	/// </summary>
	public static float HalfCos(float cos) {
		if (cos < -1 || cos > 1) return 0;
		float a = (1 + cos) / 2;
		a = (a > 0) ? a : -a;           // 取絕對值
		return Mathf.Sqrt(a);
		//return (cos < 0) ? -halfCos : halfCos;
	}
	/// <summary>
	/// 輸入 Cosin 值，回傳半角的 Sin 值 (回傳值恆為正)
	/// </summary>
	public static float HalfSin(float cos) {
		if (cos < -1 || cos > 1) return 0;
		float a = (1 - cos) / 2;
		a = (a > 0) ? a : -a;           // 取絕對值
		return Mathf.Sqrt(a);
	}

	public static Quaternion ForwardToQuaternion(Vector3 forward, Vector3 initForward = default(Vector3)) {
		if (initForward == default(Vector3)) {
			initForward = Vector3.forward;
		}

		float cos = Vector3.Dot(initForward, forward);
		float s = HalfSin(cos);
		Vector3 cross = Vector3.Cross(initForward, forward).normalized;

		return new Quaternion(cross.x * s, cross.y * s, cross.z * s, HalfCos(cos));
	}
}



[System.Serializable]
public struct BoolWraper {
	public bool[] a;
}

[System.Serializable]
public struct Vector3Wraper {
	public List<Vector3> a;
}

