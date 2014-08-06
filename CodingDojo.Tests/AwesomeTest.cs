using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace CodingDojo.Tests
{
	[TestClass]
	public class HikerTest
	{
		Banker banker;

		[TestInitialize]
		public void Initialize()
		{
			banker = new Banker();
		}

		[TestMethod]
		public void BreakTwoCents()
		{
			IList<CoinCollection> changes = banker.FindBreakingWays(2).ToList();
			Assert.AreEqual(1, changes.Count);
			Assert.AreEqual(2, changes[0][Coin.Penny]);
		}

		[TestMethod]
		public void BreakSixCents()
		{
			IList<CoinCollection> changes = banker.FindBreakingWays(6).ToList();
			Assert.AreEqual(2, changes.Count);
			Assert.AreEqual(1, changes[1][Coin.Nickel]);
			Assert.AreEqual(1, changes[1][Coin.Penny]);
		}

		[TestMethod]
		public void BreakElevenCents()
		{
			IList<CoinCollection> changes = banker.FindBreakingWays(11).ToList();
			Assert.AreEqual(4, changes.Count);
		}

		[TestMethod]
		public void Break30Cents()
		{
			IList<CoinCollection> changes = banker.FindBreakingWays(30).ToList();
			Assert.AreEqual(18, changes.Count);
		}

		[TestMethod]
		public void BreakAHundred()
		{
			IEnumerable<CoinCollection> changes = banker.FindBreakingWays(100);
			Assert.AreEqual(changes.Count(), 242);
		}
	}

	public enum Coin
	{
		Penny = 1,
		Nickel = 5,
		Dime = 10,
		Quater = 25
	}

	class Banker
	{
		CoinBreaker chiefBreaker;

		public Banker()
		{
			var largestCoinType = Enum.GetValues(typeof(Coin)).Cast<Coin>().Max();
			chiefBreaker = new CoinBreaker(largestCoinType);
		}

		public IEnumerable<CoinCollection> FindBreakingWays(int cents)
		{
			return chiefBreaker.FindBreakingWays(cents);
		}
	}

	class CoinBreaker
	{
		private static Coin[] _coinTypes = Enum.GetValues(typeof(Coin)).Cast<Coin>().OrderByDescending(x => x).ToArray();

		private CoinBreaker _nextBreaker;

		public CoinBreaker(Coin coin)
		{
			CurrentCoinType = coin;
			NextCoinType = _coinTypes.FirstOrDefault(x => x < CurrentCoinType);
			if (NextCoinType > 0)
			{
				_nextBreaker = new CoinBreaker(NextCoinType);
			}
		}

		public Coin NextCoinType { get; private set; }

		public Coin CurrentCoinType { get; private set; }

		public IEnumerable<CoinCollection> FindBreakingWays(int cents)
		{
			int numberOfCoin = 0;
			int coinValue = (int)CurrentCoinType;
			var remainingAmount = cents;
			do
			{
				if (_nextBreaker == null)
				{
					yield return BreakAllRemaing(cents);
					break;
				}
				foreach (var item in _nextBreaker.FindBreakingWays(remainingAmount))
				{
					item[CurrentCoinType] = numberOfCoin;
					yield return item;
				}
				numberOfCoin++;
				remainingAmount -= coinValue;
			}
			while (remainingAmount >= 0);
		}

		private CoinCollection BreakAllRemaing(int cents)
		{
			CoinCollection collection = new CoinCollection();
			collection[CurrentCoinType] = cents / (int)CurrentCoinType;
			return collection;
		}
	}

	class CoinCollection
	{
		private Dictionary<Coin, int> _coins;

		public CoinCollection()
		{
			_coins = new Dictionary<Coin, int>();
		}

		public int this[Coin index]
		{
			get
			{
				return _coins[index];
			}
			set
			{
				_coins[index] = value;
			}
		}

		public override string ToString()
		{
			return string.Concat(_coins.Where(x => x.Value > 0).OrderByDescending(x => x.Key).Select(x => x.Value + " " + x.Key + " "));
		}
	}
}
