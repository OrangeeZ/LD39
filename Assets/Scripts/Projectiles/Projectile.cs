using Packages.EventSystem;
using UnityEngine;
using UnityEngine.Serialization;

public class Projectile : AObject
{
    public float Lifetime;

    public float Damage { get; protected set; }

    public float LifeFraction
    {
        get { return _timer.ValueNormalized; }
    }

    public float weight = 1f;

    public bool DelayedDestroy = false;

    public bool NeedsSplashEffect = false;

    [SerializeField]
    private bool _canRedirectProjectiles = false;

    private AutoTimer _timer;

    protected Vector3 Direction;

    public Character Owner { get; protected set; }

    protected float Speed;
    protected bool CanFriendlyFire;
    private float _splashRange;

    private bool _isDestroyed = false;
    private int _frameCountStart;

    private void Awake()
    {
        enabled = false;
    }

    protected virtual void Update()
    {
        if (!_isDestroyed && _timer.ValueNormalized >= 1f)
        {
            OnLifetimeExpire();
        }

        position += Direction * Speed * Time.deltaTime;

        if (_isDestroyed && DelayedDestroy && Time.frameCount - _frameCountStart >= 4)
        {
            Destroy(gameObject);
        }
    }

    public void Launch(Character owner, Vector3 direction, float speed, float damage, bool canFriendlyFire,
        float splashRange)
    {
        this.Owner = owner;
        this.Speed = speed;
        this.Direction = direction;
        this.Damage = damage;
        this.CanFriendlyFire = canFriendlyFire;

        _splashRange = splashRange;

        transform.position = this.Owner.Pawn.GetWeaponPosition();
        transform.rotation = Quaternion.FromToRotation(Vector3.right, direction);

        _timer = new AutoTimer(Lifetime);

        var colorer = GetComponent<ProjectileColorer>();
        if (colorer != null)
        {
            colorer.Apply(Owner.IsEnemy());
        }

        enabled = true;
    }

    public virtual void OnHit(Collider other)
    {
        Release(other);
    }

    public virtual void OnContact(Collider other)
    {
    }

    public virtual void OnLifetimeExpire()
    {
        Release(null);
    }

    protected virtual void Release(Collider other)
    {
        if (!_splashRange.IsNan() && _splashRange > 0f)
        {
            Helpers.DoSplashDamage(transform.position, _splashRange, Damage, 
                teamToSkip: CanFriendlyFire ? -1 : Owner.TeamId);

            if (NeedsSplashEffect)
            {
                EventSystem.RaiseEvent(new Helpers.SplashDamage {position = position, radius = _splashRange * 0.5f});
            }
        }

        _isDestroyed = true;
        _frameCountStart = Time.frameCount;

        if (!DelayedDestroy)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isDestroyed)
        {
            return;
        }

        var otherPawn = other.GetComponent<CharacterPawnBase>();
        
        if (otherPawn != null && otherPawn != Owner.Pawn && otherPawn.Character != null)
        {
            var canAttackTarget = CanFriendlyFire || otherPawn.Character.TeamId != Owner.TeamId;

            if (canAttackTarget)
            {
                Debug.Log(otherPawn);

                otherPawn.Character.Damage(Damage);

                OnContact(other);
                OnHit(other);
            }

            return;
        }

        if (_canRedirectProjectiles)
        {
            var otherProjectile = other.GetComponent<Projectile>();
            otherProjectile?.SetOwner(Owner);
            otherProjectile?.Reflect();
            
            return;
        }

        OnContact(other);

        //if ( other.transform.parent != null ) 
        {
            var environmentObject = other.GetComponent<EnvironmentObjectSpot>();
            if (environmentObject != null)
            {
                environmentObject.Destroy(Owner);
            }
        }

        if (other.CompareTag("Environment"))
        {
            OnHit(other);
        }
    }

    private void SetOwner(Character newOwner)
    {
        Owner = newOwner;
    }

    private void Reflect()
    {
        Direction *= -1;
        transform.rotation = Quaternion.FromToRotation(Vector3.right, Direction);
    }
}