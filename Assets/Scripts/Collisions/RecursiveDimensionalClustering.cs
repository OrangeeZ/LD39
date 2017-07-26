using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Monads;

namespace Utility.Collisions {

	public class RecursiveDimensionalClustering {

		public struct CollisionPair {

			public SimpleSphereCollider a;
			public SimpleSphereCollider b;

		}

		private enum Axis {

			X,
			Y,
			Z,
			Invalid

		}

		private enum BoundaryType {

			Opening,
			Closing

		}

		private struct Boundary {

//}: IComparable<Boundary> {

			public BoundaryType boundaryType;
			//public float position;
			public SimpleSphereCollider collider;

			//public int CompareTo( Boundary other ) {

			//	return position.CompareTo( other.position );
			//}

			public static bool operator ==( Boundary a, Boundary b ) {

				return a.boundaryType == b.boundaryType && a.collider == b.collider;
			}

			public static bool operator !=( Boundary a, Boundary b ) {
				return !( a == b );
			}

		}

		private class DuplicateKeyComparer<TKey> : IComparer<TKey> where TKey : IComparable {
			#region IComparer<TKey> Members

			public int Compare( TKey x, TKey y ) {
				var result = x.CompareTo( y );

				return result == 0 ? 1 : result;
			}

			#endregion
		}

		private readonly LinkedList<Boundary> _boundaries = new LinkedList<Boundary>();

		private readonly List<Boundary> _boundariesBuffer = new List<Boundary>();
		private readonly List<SimpleSphereCollider> _subGroupBuffer = new List<SimpleSphereCollider>();

		private readonly List<CollisionPair> _pairs = new List<CollisionPair>();

		public IEnumerable<CollisionPair> Clusterize( IList<SimpleSphereCollider> group ) {

			_pairs.Clear();

			//Clusterize( _pairs, group, Axis.X );

			return _pairs;
		}

		public IEnumerable<IList<SimpleSphereCollider>> GetGroups( IList<SimpleSphereCollider> group ) {
			
			var groups = new List<IList<SimpleSphereCollider>>();
			
			Clusterize( groups, _pairs, group, Axis.X );
			
			return groups;
		}

		private void Clusterize( List<IList<SimpleSphereCollider>> groups, List<CollisionPair> pairs, IList<SimpleSphereCollider> group, Axis axis ) {

			if ( group.Count < 10 || axis == Axis.Invalid ) {

				//BruteForceComparison( pairs, group );\
				
				groups.Add(group.ToList());
				
				return;
			}

			var boundaries = GetSortedBoundaries( group, axis );

			var bracketCount = 0;
			var subGroup = new List<SimpleSphereCollider>();
			var nextAxis = GetNextAxis( axis );
			var isSubdivided = false;

			foreach ( var each in boundaries ) {

				if ( each.boundaryType == BoundaryType.Opening ) {

					++bracketCount;

					subGroup.Add( each.collider );
				} else {

					--bracketCount;

					if ( bracketCount == 0 ) {

						if ( each != boundaries.Last() ) {

							isSubdivided = true;
						}

						Clusterize( groups, pairs, subGroup, isSubdivided ? Axis.X : nextAxis );

						subGroup.Clear();
					}
				}
			}
		}

		private void BruteForceComparison( List<CollisionPair> pairs, IList<SimpleSphereCollider> group ) {

			for ( var i = 0; i < group.Count; i++ ) {
				for ( var j = i + 1; j < group.Count; j++ ) {

					if ( group[i].Intersects( group[j] ) ) {

						var result = new CollisionPair {a = group[i], b = group[j]};

						pairs.Add( result );
					}
				}
			}
		}

		private IEnumerable<Boundary> GetSortedBoundaries( IEnumerable<SimpleSphereCollider> group, Axis axis ) {

			var boundariesBuffer = new SortedList<float, Boundary>( new DuplicateKeyComparer<float>() );

			foreach ( var each in group ) {

				var pivot = GetPosition( each, axis );

				boundariesBuffer.Add( pivot - each.radius * 0.5f, new Boundary {boundaryType = BoundaryType.Opening, collider = each} );
				boundariesBuffer.Add( pivot + each.radius * 0.5f, new Boundary {boundaryType = BoundaryType.Closing, collider = each} );
			}

			return boundariesBuffer.Values;
		}

		private float GetPosition( SimpleSphereCollider sphereCollider, Axis axis ) {

			return sphereCollider.transform.position[(int) axis];
		}

		private Axis GetNextAxis( Axis axis ) {

			if ( axis == Axis.Invalid ) {

				return axis;
			}

			var nextAxis = (Axis) ( (int) axis + 1 );
			return nextAxis;
		}

	}

}