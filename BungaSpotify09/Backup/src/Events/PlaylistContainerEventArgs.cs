/*
Copyright (c) 2009 Jonas Larsson, jonas@hallerud.se

Permission is hereby granted, free of charge, to any person
obtaining a copy of this software and associated documentation
files (the "Software"), to deal in the Software without
restriction, including without limitation the rights to use,
copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the
Software is furnished to do so, subject to the following
conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
OTHER DEALINGS IN THE SOFTWARE.
*/

using System;

namespace Spotify
{
	public class PlaylistContainerEventArgs : EventArgs
	{
		private Playlist playlist;
		private int position;
		private int newPosition;
		private Playlist[] currentLists;
		
		internal PlaylistContainerEventArgs(Playlist playlist, int position, int newPosition, Playlist[] currentLists)
		{
			this.playlist = playlist;
			this.position = position;
			this.newPosition = newPosition;
			this.currentLists = currentLists;
		}
		
		public Playlist Playlist
		{
			get
			{
				return playlist;
			}
		}
		
		public int Position
		{
			get
			{
				return position;	
			}
		}
		
		public int NewPosition
		{
			get
			{
				return newPosition;	
			}
		}
		
		public Playlist[] CurrentLists
		{
			get
			{
				return currentLists;
			}
		}		
		
	}
}
