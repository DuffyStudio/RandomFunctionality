using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenLevel : MonoBehaviour {


	private int MAP_SIZE = 300;
	private int MAX_ROOMS = 100;
	private int MIN_ROOMS = 10;
	private int MAX_ROOM_WIDTH = 30;
	private int MIN_ROOM_WIDTH = 8;
	private int MAX_HALL_WIDTH = 5;
	private int MIN_HALL_WIDTH = 3;

	private List<FloorChunk> floorParams = new List<FloorChunk>();

	class IntersectPoint
	{
		private int x, z; 
		private bool xNeg, zNeg;

		public IntersectPoint() {
			x = 0;
			z = 0;
			xNeg = false;
			zNeg = false;
		}

		public IntersectPoint(int x, int z, bool xNeg, bool zNeg)
		{
			this.x = x;
			this.z = z;
			this.xNeg = xNeg;
			this.zNeg = zNeg;
		}

		public int getCenterX(){
			return this.x;
		}
		public void setCenterX(int x) {
			this.x = x;
		}
		public int getCenterZ()
		{
			return this.z;
		}
		public void setCenterZ(int z)
		{
			this.z = z;
		}
		public bool getXNeg()
		{
			return this.xNeg;
		}
		public void setXNeg(bool xNeg)
		{
			this.xNeg = xNeg;
		}
		public bool getZNeg()
		{
			return this.zNeg;
		}
		public void setZNeg(bool zNeg)
		{
			this.zNeg = zNeg;
		}
	}

	class FloorChunk
	{
		private int xP, zP, xS, zS;

		public FloorChunk()
		{
			xP = 0;
			zP = 0;
			xS = 0;
			zS = 0;
		}

		public FloorChunk(int centerX,int centerZ, int width, int depth)
		{
			this.xP = centerX;
			this.zP = centerZ;
			this.xS = width;
			this.zS = depth;
		}

		public int getCenterX() {
			return this.xP;
		}

		public void setCenterX(int centerX) {
			this.xP = centerX;
		}

		public int getCenterZ()
		{
			return this.zP;
		}

		public void setCenterZ(int centerZ)
		{
			this.zP = centerZ;
		}

		public int getWidth()
		{
			return this.xS;
		}

		public void setWidth(int width)
		{
			this.xP = width;
		}

		public int getDepth()
		{
			return this.zS;
		}

		public void setDepth(int Depth)
		{
			this.zS = Depth;
		}

		public IntersectPoint isIntersected(FloorChunk existingChunk)
		{
			int xIntersect = -1;
			int zIntersect = -1;
			bool xNeg = false;
			bool zNeg = false;

			int xLowReach = this.getCenterX() - (this.getWidth()/ 2);
			int xHighReach = this.getCenterX() + (this.getWidth() / 2);
			int zLowReach = this.getCenterZ() - (this.getDepth() / 2);
			int zHighReach = this.getCenterZ() + (this.getDepth() / 2);

			int xLowReachExist = existingChunk.getCenterX() - (existingChunk.getWidth() / 2);
			int xHighReachExist = existingChunk.getCenterX() + (existingChunk.getWidth() / 2);
			int zLowReachExist = existingChunk.getCenterZ() - (existingChunk.getDepth() / 2);
			int zHighReachExist = existingChunk.getCenterZ() + (existingChunk.getDepth() / 2);

			if (xLowReachExist >= xLowReach && xLowReachExist <= xHighReach) {
				xIntersect = xLowReachExist;
				xNeg = false;
			}
			else if (xHighReachExist >= xLowReach && xHighReachExist <= xHighReach) {
				xIntersect = xHighReachExist;
				xNeg = true; 
			}

			if (xIntersect != -1) {
				if (zLowReachExist >= zLowReach && zLowReachExist <= zHighReach)
				{
					zIntersect = zLowReachExist;
					zNeg = false;
				}
				else if (zHighReachExist >= zLowReach && zHighReachExist <= zHighReach)
				{
					zIntersect = zHighReachExist;
					zNeg = true;
				}
			}

			return new IntersectPoint(xIntersect, zIntersect, xNeg, zNeg);
		}
	}

	void Start () {
		makeRooms ();
		MakeFloor ();
	}
		
	private void makeRooms() {
		int numRooms = Random.Range(MIN_ROOMS, MAX_ROOMS);
		for (int i = 0; i < numRooms; i++) {
			int xCenter = (Random.Range(MAX_ROOM_WIDTH / 2, MAP_SIZE -  MAX_ROOM_WIDTH / 2)/2)*2;
			int zCenter = (Random.Range(MAX_ROOM_WIDTH / 2, MAP_SIZE - MAX_ROOM_WIDTH / 2)/2)*2;
			int xScale = (Random.Range(MIN_ROOM_WIDTH, MAX_ROOM_WIDTH)/2)*2;
			int zScale = (Random.Range(MIN_ROOM_WIDTH, MAX_ROOM_WIDTH)/2)*2; ;
			FloorChunk chunk = new FloorChunk(xCenter, zCenter, xScale, zScale);
			addFloorToArray(chunk);
		}
	}
	private void makeHalls() { }

	private void addFloorToArray(FloorChunk chunk) {

		foreach (FloorChunk existingChunk in floorParams)
		{
			IntersectPoint conflict = chunk.isIntersected(existingChunk);
			if (conflict.getCenterX() != -1 && conflict.getCenterZ() != -1) {
				//breakFloorOverPoint(chunk, conflict);
				//return;
			}
		}

		floorParams.Add(chunk);
	}

	private void breakFloorOverPoint(FloorChunk floor, IntersectPoint point) {
		int centerX1=0;
		int scaleX1 = 0;
		int centerZ1 = floor.getCenterZ();
		int scaleZ1 = floor.getDepth();

		if (point.getXNeg())
		{
			scaleX1 = point.getCenterX() - floor.getCenterX() - floor.getWidth() / 2;
			centerX1 = (point.getCenterX() + floor.getCenterX() - floor.getWidth() / 2)/2;
		}
		else 
		{
			scaleX1 = (floor.getCenterX() + floor.getWidth() / 2)-point.getCenterX();
			centerX1 = ((floor.getCenterX() + floor.getWidth() / 2) + point.getCenterX())/2;
		}
		addFloorToArray(new FloorChunk(centerX1, centerZ1, scaleX1, scaleZ1));

		int centerX2 = 0;
		if (point.getXNeg())
		{
			centerX2 = (centerX1 + floor.getCenterX() - floor.getWidth() / 2) / 2;
		}
		else 
		{
			centerX2 = ((centerX1 + floor.getWidth() / 2) + point.getCenterX()) / 2;
		}
		int scaleX2 = floor.getWidth() - scaleX1;
		int centerZ2 = 0;
		int scaleZ2 = 0;

		if (point.getZNeg())
		{
			scaleZ2 = point.getCenterZ() - floor.getCenterZ() - floor.getDepth() / 2;
			centerZ2 = (point.getCenterZ() + floor.getCenterZ() - floor.getDepth() / 2) / 2;
		}
		else
		{
			scaleZ2 = (floor.getCenterZ() + floor.getDepth() / 2) - point.getCenterZ();
			centerZ2 = ((floor.getCenterZ() + floor.getDepth() / 2) + point.getCenterZ()) / 2;
		}
		addFloorToArray(new FloorChunk(centerX2, centerZ2, scaleX2, scaleZ2));

	}
		
	private void MakeFloor() {
		Material mat = Resources.Load ("blue", typeof(Material)) as Material;

		foreach(FloorChunk chunk in floorParams){
			GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
			cube.transform.localScale = new Vector3(chunk.getWidth(), 1, chunk.getDepth());
			cube.transform.position = new Vector3(chunk.getCenterX(), 1, chunk.getCenterZ());
			Renderer rend = cube.GetComponent<Renderer> ();
			rend.material = mat;

		}
	}
		
}

